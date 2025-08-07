using UnityEngine;
using Pathfinding;

//chỉ dùng để kiểm tra vị trí của kẻ địch sau đó logic sẽ được xử lý trong script khác.
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

    public Transform currentEnemyTarget = null;
    public bool EnermyInAttackRange = false;// dùng để biết đang có kẻ địch trong vùng tấn công hay không
    public bool EnermyInSafeRange = false;//dung để biết có đang bị đe dọa hay không, nếu có thì sẽ rút lui
    public bool EnermyInViewRange = false; // dùng để biết có thấy kẻ địch hay không, nếu thấy thì sẽ chay lai
    public Transform closestEnemy = null;

    void Start()
    {
        aiPath = GetComponent<AIPath>();

        timer = waitTime;

        if (centerPoint == null)
            centerPoint = transform;
    }

    void Update()
    {
        EngageWithEnemy(); // Kiểm tra và xử lý kẻ địch mỗi frame
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

    }

    public void EngageWithEnemy()//gọi liên tục
    {
        if (currentEnemyTarget == null) return;
        float distance = Vector2.Distance(transform.position, currentEnemyTarget.position);
        EnermyInAttackRange = false;
        EnermyInSafeRange = false;
        EnermyInViewRange = false;
        if (distance <= safeRange)
        {
            EnermyInSafeRange = true;
            Debug.Log($"{gameObject.name} endanger → retreat");
            //cho vào hàm xử lý vị trí di chuyển.

        }
        else if (distance <= attackRange)
        {
            Debug.Log($"{gameObject.name} atk enermy");
            EnermyInAttackRange = true;
        }
        else if (distance >= attackRange)
        {
            EnermyInViewRange = true;
            Debug.Log($" {gameObject.name} see enermy → chase");
            //chuyển sang hàm xử lý vị trí di chuyển.
            //aiPath.destination = currentEnemyTarget.position;
        }
    }

    public void PatrolWhenNoEnemy()//gọi khi không có vị trí cần đi trong một khoảng thời gian nhất định
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

    public void PickNewWanderPoint()
    {
        float randomX = Random.Range(-width / 2f, width / 2f);
        float randomY = Random.Range(-height / 2f, height / 2f);

        Vector3 wanderPoint = centerPoint.position + new Vector3(randomX, randomY, 0);
        aiPath.destination = wanderPoint;
    }
    public void ResetTarget()
    {
        closestEnemy = null;
        EnermyInAttackRange = false;
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
