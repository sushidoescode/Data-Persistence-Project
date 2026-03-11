using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 
#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    public TMP_InputField nameInputField; 
    
    // NEW: The text box we just created
    public TextMeshProUGUI leaderboardText; 

    private void Start()
    {
        // When the Menu loads, instantly update the leaderboard
        UpdateLeaderboard();
    }

    public void UpdateLeaderboard()
    {
        if (DataManager.Instance == null || leaderboardText == null) return;

        // Start with a header
        string board = "TOP 5 PLAYERS\n\n";

        // Loop through however many scores we have saved (up to 5)
        for (int i = 0; i < DataManager.Instance.topScores.Count; i++)
        {
            // Format: "1. Name - 100"
            board += $"{i + 1}. {DataManager.Instance.topPlayers[i]} - {DataManager.Instance.topScores[i]}\n";
        }

        // Apply the formatted string to the UI
        leaderboardText.text = board;
    }

    public void StartNew()
    {
        if (DataManager.Instance != null && nameInputField != null)
        {
            DataManager.Instance.currentPlayerName = nameInputField.text;
        }
        SceneManager.LoadScene(1); 
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}