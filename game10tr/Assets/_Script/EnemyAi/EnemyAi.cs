using UnityEngine;
using Pathfinding;

public class EnemyAIMoveToSafeAtkCheckRange : MonoBehaviour
{
    [Header("Tầm tấn công")]
    public float attackRange = 5f;

    [Header("Tìm mục tiêu mỗi bao lâu (giây)")]
    public float checkInterval = 1.5f;

    [Header("Vùng phát hiện kẻ địch")]
    public float detectRange = 20f;

    [Header("Enemy Layer")]
    public string targetLayer = "Player";

    // ====== Nội bộ ======
    private float enemyCheckTimer;
    private AIPath aiPath;

    public Transform currentEnemyTarget = null;
    public bool EnemyInAttackRange = false;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        enemyCheckTimer = checkInterval;
    }

    void Update()
    {
        enemyCheckTimer -= Time.deltaTime;

        if (enemyCheckTimer <= 0f)
        {
            FindClosestEnemy();
            enemyCheckTimer = checkInterval;
        }

        CheckAttackRange();
    }

    void FindClosestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectRange, LayerMask.GetMask(targetLayer));
        Transform closest = null;
        float closestDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = enemy.transform;
            }
        }

        currentEnemyTarget = closest;
    }

    void CheckAttackRange()
    {
        EnemyInAttackRange = false;

        if (currentEnemyTarget == null) return;

        float distance = Vector2.Distance(transform.position, currentEnemyTarget.position);
        if (distance <= attackRange)
        {
            EnemyInAttackRange = true;
            Debug.Log($"{gameObject.name} attack enemy");
        }
    }

    public void ResetTarget()
    {
        currentEnemyTarget = null;
        EnemyInAttackRange = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
