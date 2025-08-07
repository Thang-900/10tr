using UnityEngine;

public class DestroyRevealEnemies : MonoBehaviour
{
    [Header("Object sẽ bị tắt khi phá hủy")]
    public GameObject[] objectToDestroy;

    [Header("Danh sách enemy sẽ được hiện ra")]
    public GameObject[] hiddenEnemies;

    private void OnDestroy()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM("Blood and Steel Loop");
        }
        // 1. huy object khác
        foreach (GameObject destroy in objectToDestroy)
        {
            Destroy(destroy);
        }

        // 2. Hiện enemy đã ẩn
        foreach (GameObject enemy in hiddenEnemies)
        {
            if (enemy != null)
                enemy.SetActive(true);
        }
    }
}
