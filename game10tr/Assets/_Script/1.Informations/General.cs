using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genneral : MonoBehaviour
{
    public float currentHealth = 100f;
    public float maxHealth = 100f;
    public float timeDie = 1f; // Thời gian trước khi chết
    public GameObject prefabDieEffect; // Hiệu ứng chết
    private bool isDead = false;

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            isDead = true;

            // Ẩn nhân vật để không thấy nữa
            
            gameObject.GetComponent<Collider2D>().enabled = false; // Tắt collider để không va chạm nữa
            gameObject.GetComponent<SpriteRenderer>().enabled = false; // Ẩn sprite renderer
            // Tạo hiệu ứng chết
            if (prefabDieEffect != null)
            {
                Instantiate(prefabDieEffect, transform.position, Quaternion.identity);
            }

            // Gọi Die sau thời gian delay
            Invoke("Die", timeDie);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
