using UnityEngine;
using UnityEngine.SceneManagement;

public class openLoseScene : MonoBehaviour
{
    public GameObject tower;
    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered && tower == null)
        {
            hasTriggered = true; // Đảm bảo chỉ gọi 1 lần
            Debug.Log("Tất cả kẻ địch đã bị tiêu diệt → thắng!");
            Camera.main.GetComponent<CameraPanZoom>().enabled = false; // Tắt camera pan zoom
            Invoke("open", 2f);
        }
    }

    private void open()
    {
        SceneManager.LoadScene("_LoseScene");
    }
}
