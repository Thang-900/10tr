using UnityEngine;

public class ClassSelectionData : MonoBehaviour
{
    public static ClassSelectionData Instance;

    public string className;
    public float damage;
    public float health;
    public float goldPerSecond;
    public float speed;

    void Awake()
    {
        // Nếu đã có instance thì hủy cái mới
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Gán instance và không hủy khi chuyển scene
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
