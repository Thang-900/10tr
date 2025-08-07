using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GrassZone : MonoBehaviour
{
    [Range(0f, 1f)] public float fadeAlpha = 0.5f; // độ mờ khi nhân vật vào cỏ
    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true; // Collider phải là trigger
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpriteRenderer sr = other.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = fadeAlpha;
                sr.color = c;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SpriteRenderer sr = other.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 1f; // trả lại độ rõ
                sr.color = c;
            }
        }
    }
}
