using UnityEngine;
using Pathfinding;

public class AIMoveToSafeAtkCheckRange : MonoBehaviour
{
    public float viewRange = 20f;        // Tầm nhìn
    public float attackRange = 5f;       // Tầm tấn công
    public float safeRange = 10f;        // Khoảng cách an toàn (lùi lại nếu enemy quá gần)
    public float hangAroundRange = 15f;  // Vùng tuần tra nếu không thấy enemy
    public string targetTag = "Enemy";   // Tag nhận diện enemy

    public Transform centerPoint;        // Tâm vùng tuần tra
    public float width = 10f;            // Chiều rộng vùng tuần tra
    public float height = 6f;            // Chiều cao vùng tuần tra
    public float waitTime = 1f;          // Thời gian chờ trước khi chọn điểm mới
    private float timer;

    private AIPath aiPath;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        timer = waitTime;
        PickNewWanderPoint();
    }

    void Update()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, viewRange, LayerMask.GetMask(targetTag));

        if (enemies.Length > 0)
        {
            Transform enemy = enemies[0].transform;
            float distance = Vector2.Distance(transform.position, enemy.position);

            if (distance <= attackRange)
            {
                aiPath.canMove = false; // Dừng để tấn công
            }
            else if (distance <= safeRange)
            {
                aiPath.canMove = true;
                Vector2 retreatDir = (Vector2)(transform.position - enemy.position).normalized;
                Vector2 retreatPos = (Vector2)transform.position + retreatDir * safeRange;
                aiPath.destination = retreatPos;
            }
            else if (distance <= viewRange)
            {
                aiPath.canMove = true;
                aiPath.destination = enemy.position;
            }
        }
        else
        {
            aiPath.canMove = true;
            PatrolWhenNoEnemy();
        }
    }

    void PatrolWhenNoEnemy()
    {
        Collider2D nearbyEnemy = Physics2D.OverlapCircle(transform.position, hangAroundRange, LayerMask.GetMask(targetTag));
        if (nearbyEnemy == null)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                PickNewWanderPoint();
                timer = waitTime;
            }
        }
    }

    void PickNewWanderPoint()
    {
        float randomX = Random.Range(-width / 2f, width / 2f);
        float randomY = Random.Range(-height / 2f, height / 2f);

        Vector3 wanderPoint = centerPoint.position + new Vector3(randomX, randomY, 0);
        aiPath.destination = wanderPoint;
    }

    void OnDrawGizmosSelected()
    {
        if (centerPoint == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(centerPoint.position, new Vector3(width, height, 0)); // Vùng tuần tra
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRange); // Vùng nhìn thấy enemy
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Vùng tấn công
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, safeRange); // Vùng cần lùi lại
    }
}
