using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class appearPrefabs : MonoBehaviour
{
    DisappearAfterAnim finishedAnimation; // Biến để lưu trữ script DisappearAfterAnim
    [SerializeField] GameObject prefabToAppear; // Prefab sẽ xuất hiện
    [SerializeField] float delay = 0f; // Thời gian prefab tồn tại
    [SerializeField] private Transform spawnPoint; // Vị trí xuất hiện của prefab
    private GameObject instance=null; // Biến để lưu trữ instance của prefab
    public void Appear()
    {
        if (prefabToAppear != null && spawnPoint != null)
        {
            GetComponent<SpriteRenderer>().enabled = false;  // Tắt gameObject hiện tại
            instance = Instantiate(prefabToAppear, spawnPoint.position, Quaternion.identity);
            if(finishedAnimation.endedAnimation)
            {
                active(); // Kích hoạt lại gameObject hiện tại
            }
        }
        else
        {
            Debug.LogWarning("Prefab hoặc vị trí xuất hiện không được thiết lập.");
        }
    }
    private void active()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
    
}
