using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bom : MonoBehaviour
{
    public GameObject bomOnHand;      // Kéo quả bom trên tay vào đây qua Inspector
    public bool isBomOnHand = false;

    private void Start()
    {
        if (bomOnHand != null)
        {
            bomOnHand.SetActive(false);   // Ban đầu giấu quả bom trên tay
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("weapons"))  // Nhớ check tag đúng chính tả trong Unity
        {
            other.gameObject.SetActive(false);  // Ẩn bom trên mặt đất

            isBomOnHand = true;

            if (bomOnHand != null)
            {
                bomOnHand.SetActive(true);    // Hiện quả bom trên tay
            }

            Debug.Log("Bom is on hand");
        }
    }
}
