using Pathfinding;
using System.Collections;
using UnityEngine;

public class HarmerDamageZone : MonoBehaviour
{
    AIMoveToSafeAtkCheckRange aIMoveToSafeAtkCheckRange; // Tham chiếu đến AIMoveToSafeAtkCheckRange để kiểm tra kẻ địch trong vùng tấn công
    public float damageZone = 5f;
    public bool canDamage = false;
    public float damageHammer; // Sát thương của búa, sẽ được tính toán lại dựa trên GetString
    private GetString classBuff;
    private bool initialized = false;

    private void Start()
    {
        aIMoveToSafeAtkCheckRange = GetComponentInParent<AIMoveToSafeAtkCheckRange>();
        canDamage = false;
    }

    void Update()
    {
        if (!initialized)
        {
            classBuff = FindObjectOfType<GetString>();
            if (classBuff != null)
            {
                if (gameObject.CompareTag("Player"))
                {
                    damageHammer += classBuff.playerDameBonusRate;
                    Debug.Log($"Player đã tính damageHammer = {damageHammer} một lần duy nhất.");
                }
                else if (gameObject.CompareTag("Enemy"))
                {
                    damageHammer += classBuff.enemyDameBonusRate;
                    Debug.Log($"Enemy đã tính damageHammer = {damageHammer} một lần duy nhất.");
                }
                initialized = true;
            }
            return;
        }


        if (canDamage)
        {
            canDamage = false;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, damageZone);
            foreach (var enemy in enemies)
            {
                if (enemy.CompareTag(aIMoveToSafeAtkCheckRange.targetLayer))
                {
                    HealthSystem enemyHealth = enemy.GetComponentInChildren<HealthSystem>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(damageHammer); // đã tính sẵn
                        Debug.Log("Đã gây sát thương " + damageHammer + " lên " + enemy.name);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageZone);
    }
}
