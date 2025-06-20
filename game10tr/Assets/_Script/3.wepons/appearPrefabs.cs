using System.Collections;
using UnityEngine;

public class appearPrefabs : MonoBehaviour
{
    [SerializeField] private GameObject prefabToAppear;
    [SerializeField] private float delay = 0f;
    [SerializeField] private Transform spawnPoint;

    private bool isSpawning = false; // Đánh dấu đang chờ animation cũ kết thúc
    public GameObject instance = null;

    public void Appear()
    {
        // Kiểm tra: đang spawning hoặc prefab chưa bị hủy => không cho tạo mới
        if (isSpawning || instance != null) return;
        
        if (prefabToAppear != null && spawnPoint != null)
        {
            spawnPoint.transform.GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            instance = Instantiate(prefabToAppear, spawnPoint.position, Quaternion.identity);

            // Tìm và gắn callback khi animation kết thúc
            DisappearAfterAnim disappear = instance.GetComponent<DisappearAfterAnim>();
            if (disappear != null)
            {
                isSpawning = true;
                disappear.onAnimationEnd = OnPrefabAnimationEnd;
            }
        }
        else
        {
            Debug.LogWarning("Prefab hoặc vị trí spawn chưa được thiết lập.");
        }
    }

    private void OnPrefabAnimationEnd()
    {
        instance = null;
        isSpawning = false;
        spawnPoint.transform.GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
