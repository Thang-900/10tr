using Pathfinding;
using UnityEngine;
using System.Collections;

public class HarmerPositions : MonoBehaviour
{
    private AIPath aiPath;
    private AIMoveToSafeAtkCheckRange atkCheck;
    private HarmerAttackDisplay harmerAttackDisplay;
    private Vector3 retreatPos;
    private bool isAttacking = false;

    public bool playerHasOrder = false;
    public Vector3 playerDestination;
    public float arrivalThreshold = 1f;
    public Transform EnemyTarget1;
    public Transform EnemyTarget2;


    void Start()
    {
        aiPath = GetComponent<AIPath>();
        atkCheck = GetComponent<AIMoveToSafeAtkCheckRange>();
        harmerAttackDisplay = GetComponent<HarmerAttackDisplay>();
        aiPath.destination = transform.position+new Vector3(0.3f,0.3f,0.3f); // Bắt đầu đứng yên

        // Đăng ký callback từ HarmerAttackDisplay
        harmerAttackDisplay.OnAttackEnd += OnAttackFinished;
    }

    private void Patrol()
    {
        atkCheck.PatrolWhenNoEnemy();
    }

    private bool Reached(Vector3 a, Vector3 b, float dist) =>
        Vector2.Distance(a, b) <= dist;

    public void IssuePlayerMoveOrder(Vector3 dest)
    {
        playerHasOrder = true;
        playerDestination = dest;
        aiPath.destination = playerDestination;
    }

    void Update()
    {
        Debug.Log(gameObject.name + " isAttacking:" + isAttacking);
        Debug.Log(gameObject.name + " playerHasOrder:" + playerHasOrder);

        // Đang tấn công thì không xử lý gì khác
        if (isAttacking) return;

        if (playerHasOrder)
        {
            aiPath.destination = playerDestination;

            // Gọi SearchPath nếu cần
            if (!aiPath.pathPending && !aiPath.reachedEndOfPath)
                aiPath.SearchPath();

            // Kiểm tra đã đến đích chưa
            if (!aiPath.pathPending && aiPath.reachedEndOfPath)
            {
                Debug.Log("Xong order of player");
                playerHasOrder = false;
            }
            return;
        }


        // Trong tầm tấn công → bắt đầu đánh
        if (atkCheck.EnermyInAttackRange && atkCheck.closestEnemy != null)
        {
            StartAttack();
            return;
        }

        // Kẻ địch quá gần → rút lui
        if (atkCheck.EnermyInSafeRange && atkCheck.closestEnemy != null)
        {
            Vector2 retreatDir = ((Vector2)transform.position - (Vector2)atkCheck.closestEnemy.position).normalized;
            retreatPos = (Vector2)transform.position + retreatDir * (atkCheck.attackRange + atkCheck.safeRange);
            aiPath.destination = retreatPos;
            return;
        }

        // Thấy kẻ địch trong tầm nhìn → lao tới
        if (atkCheck.closestEnemy != null && atkCheck.EnermyInViewRange)
        {
            aiPath.destination = atkCheck.closestEnemy.position;
            return;
        }

        else
        {
            if(gameObject.CompareTag("Enemy"))
            {
                // Nếu là kẻ địch → tuần tra
                if (EnemyTarget1 != null)
                {
                    Debug.Log("Enemy đang chạy tới EnemyTarget1");
                    aiPath.destination = EnemyTarget1.position;
                }
                else
                {
                    if (EnemyTarget2 == null)
                    {
                        Invoke(nameof(Patrol), 3);
                    }
                    else
                    {
                        Debug.Log("Enemy đang chạy tới EnemyTarget2");
                        aiPath.destination = EnemyTarget2.position;
                    }
                    
                }
            }
            else
            {
                // Nếu là người chơi → tuần tra
                Invoke(nameof(Patrol), 3);
            }
        }
    }

    private void StartAttack()
    {
        isAttacking = true;
        aiPath.destination = transform.position;
        aiPath.canMove = false;

        harmerAttackDisplay.TriggerAttack();
    }

    private void OnAttackFinished()
    {
        // Gọi khi animation đánh xong
        aiPath.canMove = true;
        isAttacking = false;
    }
}
