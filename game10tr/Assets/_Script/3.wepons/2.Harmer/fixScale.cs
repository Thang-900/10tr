using UnityEngine;

public class CopyPlayerScale : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");

        if (player == null)
            Debug.LogError("Không tìm thấy GameObject tên 'Player'!");
    }

    void Update()
    {
        if (player != null)
        {
            transform.localScale = player.transform.localScale;
        }
    }
}
