using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highestScoreText;

    private int score = 0;
    private int highestScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadHighScore();
        UpdateUI();
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Current Score: " + score);  // Kiểm tra điểm số có tăng không
        UpdateUI();  // Cập nhật giao diện ngay lập tức
    }



    public void SaveFinalScore()
    {
        if (score > highestScore)
        {
            highestScore = score;
            PlayerPrefs.SetInt("HighestScore", highestScore);
            PlayerPrefs.Save();
        }
        UpdateUI();
    }

    private void LoadHighScore()
    {
        highestScore = PlayerPrefs.GetInt("HighestScore", 0);
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        if (highestScoreText != null)
        {
            highestScoreText.text = "Highest Score: " + highestScore.ToString();
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void SaveFinalScore(int finalScore)
    {
        score = finalScore; // Cập nhật điểm cuối cùng
        if (score > highestScore)
        {
            highestScore = score;
            PlayerPrefs.SetInt("HighestScore", highestScore);
            PlayerPrefs.Save();
        }
        UpdateUI();
    }
    public void ResetScore()
    {
        Debug.Log("Resetting score..."); // Kiểm tra reset có hoạt động không
        score = 0;
        UpdateUI();  // Cập nhật UI ngay lập tức
    }



}
