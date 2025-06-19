using UnityEngine;

public class damageHarmer : MonoBehaviour
{
    public float damage = 50f;
    public float radius = 1.5f;

    private void Start()
    {
        // Gây sát thương khi chem
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var hit in hitColliders)
        {
            Genneral g = hit.GetComponent<Genneral>();
            if (g != null)
            {
                g.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ vòng tròn bán kính nổ trong editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
