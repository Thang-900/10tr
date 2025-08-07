using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VoDichTrigger : MonoBehaviour
{
    private bool hasTriggered = false;
    private GameObject[] Buff;
    private void Start()
    {
        Buff = GameObject.FindGameObjectsWithTag("Buff");
        foreach (GameObject obj in Buff)
        {
            obj.SetActive(false); // Tắt tất cả các đối tượng có tag "Buff" ngay từ đầu
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu đã kích hoạt rồi thì bỏ qua
        if (hasTriggered) return;

        // Kiểm tra tag hoặc layer là "Player"
        if (collision.CompareTag("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            hasTriggered = true; // Đánh dấu đã kích hoạt
            Debug.Log("Va chạm lần đầu với Player!");

            // Tìm tất cả object có tag "Player"
            GameObject[] taggedPlayers = GameObject.FindGameObjectsWithTag("Player");
            Buff = GameObject.FindGameObjectsWithTag("Buff");
            foreach (GameObject obj in taggedPlayers)
            {
                HealthSystem hs = obj.GetComponent<HealthSystem>();
                if (hs != null)
                {
                    hs.buff(); // Gọi hàm buff() nếu có HealthSystem
                }
            }
            foreach(GameObject obj in Buff)
            {
                obj.SetActive(true); // Kích hoạt tất cả các đối tượng có tag "Buff"
            }

            Debug.Log($"Tìm thấy {taggedPlayers.Length} đối tượng có tag 'Player'");

            // Tắt collider sau khi hoàn thành
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
    }
}
