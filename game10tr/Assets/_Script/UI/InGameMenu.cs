using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public MultiAIBomManager multiSaveManager;

    public void OnExitGame()
    {
        SaveBeforeQuit();
        Application.Quit(); // Chỉ có tác dụng khi build
        Debug.Log("Thoát game.");
    }

    public void OnReturnToMainMenu()
    {
        SaveBeforeQuit();
        SceneManager.LoadScene("MainMenu"); // Ghi đúng tên scene chính
    }

    private void SaveBeforeQuit()
    {
        // Lưu scene hiện tại
        PlayerPrefs.SetString("SavedScene", "GameScene");

        // Lưu toàn bộ AI
        if (multiSaveManager != null)
            multiSaveManager.SaveAll();

        PlayerPrefs.Save();
    }

}
