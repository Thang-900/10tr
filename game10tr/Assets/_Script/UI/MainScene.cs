using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel; // Giao diện cài đặt
    [SerializeField] private GameObject mainMenuPanel; // Giao diện chính
    [SerializeField] private GameObject ideaButtonsCollection;//giao diện các nút ý tưởng
    [SerializeField] private GameObject panelsCollection; // kiểm soát toàn bộ panel mô tả lý tưởng

    [SerializeField] private GameObject congDongPanel; 
    [SerializeField] private GameObject laoDongPanel; 
    [SerializeField] private GameObject cachMangPanel; 
    [SerializeField] private GameObject phiChuNghiaPanel; 
    void Start()
    {
        if (settingsPanel != null && mainMenuPanel != null&& ideaButtonsCollection!=null && congDongPanel != null && laoDongPanel!=null&&cachMangPanel!=null&&phiChuNghiaPanel!=null)
        {
            SoundManager.Instance.PlayBGM("Menu");
            EventSystem.current.SetSelectedGameObject(null);

            settingsPanel.SetActive(false); // Ẩn giao diện cài đặt khi bắt đầu
            ideaButtonsCollection.SetActive(false); // Ẩn giao diện các nút ý tưởng khi bắt đầu

            congDongPanel.SetActive(false); // Ẩn giao diện cộng đồng
            laoDongPanel.SetActive(false); // Ẩn giao diện lao động
            cachMangPanel.SetActive(false); // Ẩn giao diện cách mạng
            phiChuNghiaPanel.SetActive(false); // Ẩn giao diện phi chủ nghĩa

            mainMenuPanel.SetActive(true); // Hiển thị giao diện chính

            Debug.Log("Giao diện chính đã được khởi tạo.");
        }
        else
        {
            Debug.LogError("Vui lòng gán các panel giao diện trong Inspector trong! "+gameObject.name);
        }
    }
    public void StartNewGame()//chọn idea trước khi vào gamescene
    {
        // Xóa hết dữ liệu cũ (bao gồm cả score và scene)
        PlayerPrefs.DeleteAll();

        // Reset điểm (score) nếu có GameSession
        if (GameSession.Instance != null)
        {
            GameSession.Instance.ResetScore();
        }

        // chon ly tuong truoc khi vao game
        resetUI(); // Ẩn tất cả giao diện ;
        ideaButtonsCollection.SetActive(true); // hiện giao diện chọn ý tưởng khi bắt đầu
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
        resetUI(); // Ẩn tất cả giao diện
        settingsPanel.SetActive(true); // Hiển thị giao diện cài đặt
    }
    public void openMain()
    {         // Quay lại giao diện chính
        resetUI(); // Ẩn tất cả giao diện
        mainMenuPanel.SetActive(true); // Hiển thị giao diện chính
    }
    public void OpenChoseIdeaPanel()
    {
        resetUI(); // Ẩn tất cả giao diện
        ideaButtonsCollection.SetActive(true); // Hiển thị giao diện các nút ý tưởng
    }
    public void OpenGameplay()
    {
        // Tải scene chơi game
        SceneManager.LoadScene("GameScene");
    }
    public void OpenCongDongPanel()
    {
        resetUI(); // Ẩn tất cả giao diện
        panelsCollection.SetActive(true); // Hiển thị toàn bộ panel mô tả lý tưởng
        congDongPanel.SetActive(true); // Hiển thị giao diện cộng đồng
    }
    public void OpenLaoDongPanel()
    {
        resetUI(); // Ẩn tất cả giao diện
        panelsCollection.SetActive(true); // Hiển thị toàn bộ panel mô tả lý tưởng
        laoDongPanel.SetActive(true); // Hiển thị giao diện lao động
    }
    public void OpenCachMangPanel()
    {
        resetUI(); // Ẩn tất cả giao diện
        panelsCollection.SetActive(true); // Hiển thị toàn bộ panel mô tả lý tưởng
        cachMangPanel.SetActive(true); // Hiển thị giao diện cách mạng
    }
    public void OpenPhiChuNghiaPanel()
    {
        resetUI(); // Ẩn tất cả giao diện
        panelsCollection.SetActive(true); // Hiển thị toàn bộ panel mô tả lý tưởng
        phiChuNghiaPanel.SetActive(true); // Hiển thị giao diện phi chủ nghĩa
    }
    public void resetUI()
    {
        // Ẩn tất cả giao diện
        ideaButtonsCollection.SetActive(false); // Ẩn giao diện các nút ý tưởng
        settingsPanel.SetActive(false);// Ẩn giao diện cài đặt
        congDongPanel.SetActive(false); // Ẩn giao diện cộng đồng
        laoDongPanel.SetActive(false); // Ẩn giao diện lao động
        cachMangPanel.SetActive(false); // Ẩn giao diện cách mạng
        phiChuNghiaPanel.SetActive(false); // Ẩn giao diện phi chủ nghĩa
        mainMenuPanel.SetActive(false);// Ẩn giao diện chính
        panelsCollection.SetActive(false); // Ẩn toàn bộ panel mô tả lý tưởng
    }
}
