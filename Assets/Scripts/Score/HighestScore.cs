using UnityEngine;
using TMPro;

namespace Assets.Scripts.Score
{
    public class HighestScore : MonoBehaviour
    {
        private const string HighScoreKey = "HighScore";

        public TMP_Text highScoreText; // Kết nối với UI TextMeshPro

        private void Start()
        {
            UpdateHighScoreUI();
        }

        // Lấy điểm số cao nhất từ PlayerPrefs
        public static int GetHighScore()
        {
            return PlayerPrefs.GetInt(HighScoreKey, 0);
        }

        // Lưu điểm số cao nhất nếu điểm mới cao hơn
        public static void SaveHighScore(int score)
        {
            int currentHighScore = GetHighScore();
            if (score > currentHighScore)
            {
                PlayerPrefs.SetInt(HighScoreKey, score);
                PlayerPrefs.Save();
            }
        }

        // Reset điểm số cao nhất
        public static void ResetHighScore()
        {
            PlayerPrefs.DeleteKey(HighScoreKey);
            PlayerPrefs.Save();
        }

        // Cập nhật hiển thị UI
        private void UpdateHighScoreUI()
        {
            if (highScoreText != null)
            {
                highScoreText.text = "High Score: " + GetHighScore().ToString();
            }
        }
    }
}
