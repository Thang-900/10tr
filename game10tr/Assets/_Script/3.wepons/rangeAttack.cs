using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage = 50f;
    public float radius = 1.5f;

    private void Start()
    {
        // Gây sát thương khi nổ
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hitColliders)
        {
            if(hit.CompareTag(tag: "Player"))
            {
                continue; // Bỏ qua nếu là người chơi
            }
            Genneral g = hit.GetComponent<Genneral>();
            if (g != null)
            {
                g.TakeDamage(damage);
            }
        }
        Destroy(gameObject, 0.2f);
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ vòng tròn bán kính nổ trong editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
