using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public string currentPlayerName;

    // We now use Lists instead of single variables
    public List<int> topScores = new List<int>();
    public List<string> topPlayers = new List<string>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        LoadHighScores();
    }

    [System.Serializable]
    class SaveData
    {
        // JSON will now save these entire lists
        public List<int> savedScores;
        public List<string> savedPlayers;
    }

    public void SaveHighScores()
    {
        SaveData data = new SaveData();
        data.savedScores = topScores;
        data.savedPlayers = topPlayers;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScores()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // The '??' means: if the saved data is null, create a brand new empty list instead of crashing
            topScores = data.savedScores ?? new List<int>();
            topPlayers = data.savedPlayers ?? new List<string>();
        }
    }

    // --- THE LEADERBOARD LOGIC ---
    public void AddScore(string playerName, int score)
    {
        // 1. Add the new score to the lists
        topScores.Add(score);
        topPlayers.Add(playerName);

        // 2. Use LINQ to tie the names and scores together, and sort them from highest to lowest
        var combined = topPlayers.Zip(topScores, (p, s) => new { Player = p, Score = s })
                                 .OrderByDescending(x => x.Score)
                                 .ToList();

        // 3. Separate them back into our individual lists
        topPlayers = combined.Select(x => x.Player).ToList();
        topScores = combined.Select(x => x.Score).ToList();

        // 4. If the list is larger than 5, remove the lowest one at the bottom!
        if (topScores.Count > 5)
        {
            topScores.RemoveAt(5);
            topPlayers.RemoveAt(5);
        }

        // 5. Save the new top 5 to the hard drive
        SaveHighScores();
    }

    // Helper methods so our MainManager can easily grab the #1 spot to display
    public int GetHighestScore()
    {
        return topScores.Count > 0 ? topScores[0] : 0;
    }

    public string GetHighestPlayer()
    {
        return topPlayers.Count > 0 ? topPlayers[0] : "None";
    }
}