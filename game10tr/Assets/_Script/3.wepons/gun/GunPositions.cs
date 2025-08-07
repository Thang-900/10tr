using Pathfinding;
using UnityEngine;
using System.Collections;

public class GunnerPositions : MonoBehaviour
{
    private AIPath aiPath;
    private AIMoveToSafeAtkCheckRange atkCheck;
    private BulletCount bulletCount;
    private GunnerAttackDisplay gunnerAttackDisplay;
    private Vector2 retreatPos;
    private bool isReloading = false;
    private bool storedOldPosition = false;
    private Vector3 oldPosition;

    public bool playerHasOrder = false;
    public Vector3 playerDestination;
    public float arrivalThreshold = 0.1f;
    public Transform EnemyTarget1;
    public Transform EnemyTarget2;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        atkCheck = GetComponent<AIMoveToSafeAtkCheckRange>();
        bulletCount = GetComponent<BulletCount>();
        gunnerAttackDisplay = GetComponent<GunnerAttackDisplay>();

        aiPath.destination = transform.position + new Vector3(0.5f, 0.5f, 0); // Bắt đầu đứng yên

        // Gắn callback khi animation bắn kết thúc
        if (gunnerAttackDisplay != null)
            gunnerAttackDisplay.OnAttackEnd += OnAttackFinished;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("BulletStore") || other.CompareTag("EnemyBulletStore") && bulletCount.outOfBullet && !isReloading)
        {
            Debug.Log("Đang nạp đạn tại kho đạn");
            isReloading = true;
            gunnerAttackDisplay.changeDirection = true;
            StartCoroutine(DelayResetBullet());
        }
    }

    private IEnumerator DelayResetBullet()
    {
        yield return new WaitForSeconds(2f);
        bulletCount.ResetBulletCount();
        isReloading = false;
        if (!gameObject.CompareTag("Enemy"))
        {
            aiPath.destination = oldPosition; // Trở về vị trí cũ sau khi nạp đạn

            storedOldPosition = false; // Đặt lại trạng thái lưu vị trí cũ
        }
        Debug.Log("Đã nạp đạn xong. outOfBullet? " + bulletCount.outOfBullet);
    }

    private void Patrol()
    {
        atkCheck.PatrolWhenNoEnemy();
    }

    public void IssuePlayerMoveOrder(Vector3 dest)
    {
        // Nếu đang tấn công, lệnh di chuyển sẽ bị hoãn đến khi bắn xong
        if (gunnerAttackDisplay != null && gunnerAttackDisplay.isAttacking)
        {
            Debug.Log("Đang tấn công, lệnh di chuyển sẽ chờ đến khi bắn xong");
            StartCoroutine(WaitAndMoveAfterAttack(dest));
            return;
        }

        playerHasOrder = true;
        playerDestination = dest;
        aiPath.destination = playerDestination;
    }

    private IEnumerator WaitAndMoveAfterAttack(Vector3 dest)
    {
        // Chờ khi bắn xong
        while (gunnerAttackDisplay != null && gunnerAttackDisplay.isAttacking)
        {
            yield return null;
        }
        IssuePlayerMoveOrder(dest);
    }

    private void OnAttackFinished()
    {
        // Logic sau khi animation bắn xong (nếu cần)
        Debug.Log("Animation bắn xong → tiếp tục logic di chuyển/tấn công.");
    }

    void Update()
    {

        // Nếu đang nhận lệnh của người chơi
        if (playerHasOrder)
        {
            Debug.Log("Gunner has order from player");
            aiPath.destination = playerDestination;

            if (!aiPath.pathPending && !aiPath.reachedEndOfPath)
                aiPath.SearchPath();

            if (!aiPath.pathPending && aiPath.reachedEndOfPath)
            {
                Debug.Log("Xong order of player");
                playerHasOrder = false;
                Debug.Log("Đã đến đích của người chơi, đứng yên tại vị trí hiện tại.");
            }
            return;
        }

        // Nếu đang bắn → đứng yên, không xử lý logic khác
        else if (gunnerAttackDisplay != null && gunnerAttackDisplay.isAttacking)
        {
            aiPath.destination = transform.position;
            return;
        }

        if (bulletCount == null || aiPath == null || atkCheck == null) return;

        // Khi hết đạn và chưa nạp xong → đến kho đạn
        else if (bulletCount.outOfBullet && !isReloading)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (!storedOldPosition)
                {
                    oldPosition = transform.position;
                    storedOldPosition = true;
                }
                Transform nearestEnemyStore = PoolBomBullet.Instance.GetNearestEnemyBulletStore(transform.position);
                if (nearestEnemyStore != null)
                {
                    aiPath.destination = nearestEnemyStore.position;
                }
                return;
            }
            else
            {
                if (!storedOldPosition)
                {
                    oldPosition = transform.position;
                    storedOldPosition = true;
                }
                Transform nearestPlyerStore = PoolBomBullet.Instance.GetNearestBulletStore(transform.position);
                if (nearestPlyerStore != null)
                {
                    aiPath.destination = nearestPlyerStore.position;
                }
                return;
            }
        }

        // Khi kẻ địch quá gần → rút lui
        else if (atkCheck.EnermyInSafeRange && atkCheck.closestEnemy != null)
        {
            Vector2 retreatDir = ((Vector2)transform.position - (Vector2)atkCheck.closestEnemy.position).normalized;
            retreatPos = (Vector2)transform.position + retreatDir * (atkCheck.attackRange + atkCheck.safeRange);
            aiPath.destination = retreatPos;
            return;
        }

        // Khi trong tầm tấn công → đứng yên để bắn
        else if (atkCheck.EnermyInAttackRange)
        {
            aiPath.destination = transform.position;
            return;
        }

        // Khi thấy kẻ địch → di chuyển tới
        else if (!bulletCount.outOfBullet && atkCheck.closestEnemy != null && atkCheck.EnermyInViewRange)
        {
            Debug.Log("Di chuyển tới kẻ địch");
            aiPath.destination = atkCheck.closestEnemy.position;
            return;
        }

        // Không có gì → tuần tra
        else
        {
            //neu la enemy
            if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
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
                // neu la player
                Invoke(nameof(Patrol), 3);
            }
        }
    }
}
