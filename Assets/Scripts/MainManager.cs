using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    private int m_BricksRemaining;
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text BestScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        if (DataManager.Instance != null)
        {
            // NEW: Pulls the #1 spot from the lists!
            BestScoreText.text = $"Best Score : {DataManager.Instance.GetHighestPlayer()} : {DataManager.Instance.GetHighestScore()}";
        }
        
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        // Calculate total bricks spawned
        m_BricksRemaining = LineCount * perLine;

        // --- THE FIX ---
        // We must define and fetch the playerName inside Start() so it has "context"
        string playerName = "Player";
        if (DataManager.Instance != null && !string.IsNullOrEmpty(DataManager.Instance.currentPlayerName))
        {
            playerName = DataManager.Instance.currentPlayerName;
        }

        // Now the script knows exactly what playerName is!
        ScoreText.text = $"{playerName} Score : {m_Points}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        m_BricksRemaining--;
        if (m_BricksRemaining <= 0)
        {
            Victory();
        }
        
        // Check if the DataManager exists to grab the name, otherwise default to "Player"
        string playerName = "Player";
        if (DataManager.Instance != null && !string.IsNullOrEmpty(DataManager.Instance.currentPlayerName))
        {
            playerName = DataManager.Instance.currentPlayerName;
        }

        // Display the active player's name next to their current score!
        ScoreText.text = $"{playerName} Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (DataManager.Instance != null)
        {
            // Just pass the player's name and score to the vault.
            // The DataManager will automatically check if it's high enough, sort it, and save it!
            DataManager.Instance.AddScore(DataManager.Instance.currentPlayerName, m_Points);

            // Update the UI just in case they took the #1 spot
            BestScoreText.text = $"Best Score : {DataManager.Instance.GetHighestPlayer()} : {DataManager.Instance.GetHighestScore()}";
        }
    }

    public void Victory()
    {
        m_GameOver = true;

        // Change the Game Over text to a winning message!
        GameOverText.GetComponent<Text>().text = "YOU WIN!\nPress Space to Restart";
        GameOverText.SetActive(true);

        if (DataManager.Instance != null)
        {
            DataManager.Instance.AddScore(DataManager.Instance.currentPlayerName, m_Points);
            BestScoreText.text = $"Best Score : {DataManager.Instance.GetHighestPlayer()} : {DataManager.Instance.GetHighestScore()}";
        }
    }

    public void BackToMenu()
    {
        // Index 0 is your Menu scene in the Build Settings!
        SceneManager.LoadScene(0); 
    }
}
