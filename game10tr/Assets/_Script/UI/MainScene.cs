using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel; // Giao diện cài đặt
    [SerializeField] private GameObject mainMenuPanel; // Giao diện chính
    void Start()
    {
        SoundManager.Instance.PlayBGM("Menu");
        settingsPanel.SetActive(false); // Ẩn giao diện cài đặt khi bắt đầu
        mainMenuPanel.SetActive(true); // Hiển thị giao diện chính
    }
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
        SceneManager.LoadScene("ChoseIdea");
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
    public void OpenSettings()
    {
        // Mở giao diện cài đặt
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    public void openMain()
    {         // Quay lại giao diện chính
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    public void OpenGameplay()
    {
        // Tải scene chơi game
        SceneManager.LoadScene("GameScene");
    }
}
