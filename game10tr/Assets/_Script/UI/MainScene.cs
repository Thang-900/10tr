using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartNewGame()
    {
        // Xóa hết dữ liệu cũ (bao gồm cả score và scene)
        PlayerPrefs.DeleteAll();

        // Reset điểm (score) nếu có GameSession
        if (GameSession.Instance != null)
        {
            GameSession.Instance.ResetScore();
        }

        // Tải scene chơi game
        SceneManager.LoadScene("GameScene");
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("SavedScene"))
        {
            string savedScene = PlayerPrefs.GetString("SavedScene");
            float savedScore = PlayerPrefs.GetFloat("SavedScore", 0f); // Lấy điểm đã lưu (nếu có)

            if (GameSession.Instance != null)
            {
                GameSession.Instance.score = savedScore; // Gán điểm lại
            }

            SceneManager.LoadScene(savedScene);
        }
        else
        {
            Debug.Log("Không có dữ liệu lưu!");
        }
    }

    public void ExitGame()
    {
        // Lưu scene hiện tại
        PlayerPrefs.SetString("SavedScene", "GameScene");

        // Lưu điểm hiện tại nếu có
        if (GameSession.Instance != null)
        {
            PlayerPrefs.SetFloat("SavedScore", GameSession.Instance.score);
        }

        PlayerPrefs.Save();

        Application.Quit();
        Debug.Log("Thoát game và đã lưu dữ liệu.");
    }
}
