using UnityEngine;
using Pathfinding;

public class AIMoveToSafeAtkCheckRange : MonoBehaviour
{
    [Header("Tầm và vùng")]
    public float viewRange = 20f;
    public float attackRange = 5f;
    public float safeRange = 3f; // nhỏ hơn attackRange
    public float hangAroundRange = 15f;


    [Header("Tuần tra")]
    public Transform centerPoint;
    public float width = 10f;
    public float height = 6f;
    public float waitTime = 1f;

    [Header("Enemy Layer")]
    public string targetLayer = "Enemy";

    // ====== Nội bộ ======
    private float timer;
    private AIPath aiPath;
    private Transform currentEnemyTarget = null;
    private bool isRunning = false;

    public bool isAtkingEnermy = false;// dùng để biết đang có kẻ địch trong vùng tấn công hay không
    public Transform closestEnemy = null;
    Vector2 retreatPos;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        timer = waitTime;

        if (centerPoint == null)
            centerPoint = transform;

        PickNewWanderPoint();
    }

    void Update()
    {
        // Nếu đang rút lui, tiếp tục di chuyển cho đến khi tới nơi
        if (isRunning)
        {
            aiPath.destination = retreatPos;

            if (Vector2.Distance(transform.position, retreatPos) < 0.5f)
                isRunning = false;

            return;
        }

        // Tìm enemy gần nhất trong vùng nhìn
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, viewRange, LayerMask.GetMask(targetLayer));
        closestEnemy = null;
        float closestDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestEnemy = enemy.transform;
            }
        }

        currentEnemyTarget = closestEnemy;

        if (currentEnemyTarget == null)
        {
            PatrolWhenNoEnemy();
        }
        else
        {
            EngageWithEnemy();
        }
    }

    void EngageWithEnemy()
    {
        float distance = Vector2.Distance(transform.position, currentEnemyTarget.position);

        if (distance <= safeRange)
        {
            Debug.Log("enemy is in white range → retreat");
            aiPath.canMove = true;
            isAtkingEnermy = false;
            Vector2 retreatDir = (Vector2)(transform.position - currentEnemyTarget.position).normalized;
            retreatPos = (Vector2)transform.position + retreatDir * (attackRange + safeRange);
            isRunning = true;
        }
        else if (distance <= attackRange)
        {
            Debug.Log("enemy is in red range → stop to attack");
            isAtkingEnermy = true;
            if (aiPath.canMove)
            {
                aiPath.canMove = false;
                aiPath.destination = transform.position; // Dừng lại tại vị trí hiện tại

            }
        }
        else
        {
            isAtkingEnermy = false;
            Debug.Log("enemy is in blue range → chase");
            if (!aiPath.canMove)
                aiPath.canMove = true;

            aiPath.destination = currentEnemyTarget.position;
        }
    }

    void PatrolWhenNoEnemy()
    {
        if (!aiPath.canMove)
            aiPath.canMove = true;

        timer -= Time.deltaTime;

        if (!aiPath.pathPending && aiPath.reachedEndOfPath && timer <= 0f)
        {
            PickNewWanderPoint();
            timer = waitTime;
        }
    }

    void PickNewWanderPoint()
    {
        float randomX = Random.Range(-width / 2f, width / 2f);
        float randomY = Random.Range(-height / 2f, height / 2f);

        Vector3 wanderPoint = centerPoint.position + new Vector3(randomX, randomY, 0);
        aiPath.destination = wanderPoint;
    }
    public void ResetTarget()
    {
        closestEnemy = null;
        isAtkingEnermy = false;
    }

    void OnDrawGizmosSelected()
    {
        if (centerPoint == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(centerPoint.position, new Vector3(width, height, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, safeRange);
    }
}
