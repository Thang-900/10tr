using UnityEngine;
using UnityEngine.SceneManagement;

public class ToweDestroy : MonoBehaviour
{
    private bool gameEnded = false; // Đảm bảo chỉ chạy 1 lần
    public float checkInterval = 2f; // Kiểm tra mỗi 1 giây
    private float timer = 0f;

    
    private void OnDestroy()
    {
        if (!gameEnded)
        {
            gameEnded = true;
            if(Camera.main != null)
            {
                Camera.main.transform.position = transform.position+new Vector3(0,0,-10); // Đặt camera vào vị trí của towe bị phá hủy
            }
            else
            {
                Debug.LogWarning("Không tìm thấy Camera chính!");
            }
            
        }
    }
    private void Update()
    {
        if (gameEnded) return;

        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            timer = 0f;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            bool hasEnemyAlive = false;
            foreach (var e in enemies)
            {
                if (e.activeInHierarchy) // bỏ qua enemy đã bị disable
                {
                    hasEnemyAlive = true;
                    break;
                }
            }

            if (!hasEnemyAlive)
            {
                gameEnded = true;
                Debug.Log("Tất cả kẻ địch đã bị tiêu diệt → thắng!");
                SceneManager.LoadScene("_VictoryScene");
                // hoặc winPanel.SetActive(true);
            }
        }
    }
}
