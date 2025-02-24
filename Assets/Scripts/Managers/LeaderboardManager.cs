using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public TMP_Text[] scoreTexts; 
    public GameObject HighScorePanel;

    void Start()
    {
        LoadHighScores(); 
    }

    public void ShowHighScores()
    {
        HighScorePanel.SetActive(true);
        LoadHighScores(); 
    }

    public void SaveHighScore(int newScore)
    {
        List<int> highScores = new List<int>();

        for (int i = 0; i < 10; i++)
        {
            highScores.Add(PlayerPrefs.GetInt("HighScore" + i, 0));
        }

        highScores.Add(newScore);
        highScores.Sort((a, b) => b.CompareTo(a)); 

        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.SetInt("HighScore" + i, highScores[i]);
        }
        PlayerPrefs.Save();
    }

    void LoadHighScores()
    {
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            int score = PlayerPrefs.GetInt("HighScore" + i, 0);
            scoreTexts[i].text = (i + 1) + ". " + score;
        }
    }

    public void ResetHighScores()
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.DeleteKey("HighScore" + i);
        }
        PlayerPrefs.Save();
        LoadHighScores();
    }
}
