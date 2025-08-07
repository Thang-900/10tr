using Pathfinding;
using UnityEngine;
using System.Collections;

public class BomAIPositions : MonoBehaviour
{
    [Header("Player order")]
    public bool playerHasOrder = false;
    public Vector3 playerDestination;
    public float arrivalThreshold = 0.1f;
    public Transform EnemyTarget1;
    public Transform EnemyTarget2;

    private AIPath aiPath;
    private AIMoveToSafeAtkCheckRange atkCheck;
    private GunnerAttackDisplay gunnerAttackDisplay;
    private BomAnimation bomAnimation;


    private Vector3 oldPosition;
    private bool storedOldPosition = false;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        atkCheck = GetComponent<AIMoveToSafeAtkCheckRange>();
        bomAnimation = GetComponent<BomAnimation>();
        gunnerAttackDisplay = GetComponent<GunnerAttackDisplay>();
        aiPath.destination = transform.position+new Vector3(0.5f,0.5f,0); // Bắt đầu đứng yên
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("BomStore")|| other.CompareTag("EnemyBomStore") && !bomAnimation.isHoldingBom)
        {
            Debug.Log("Đang nạp bom tại kho");
            StartCoroutine(DelayResetBullet());
        }
    }

    private IEnumerator DelayResetBullet()
    {
        yield return new WaitForSeconds(2.3f);
        bomAnimation.isHoldingBom = true;
        storedOldPosition = false;
        Debug.Log("Đã nạp bom xong " + bomAnimation.isHoldingBom);

        // Nếu vẫn còn lệnh người chơi → quay lại chạy tới điểm đó
        if (playerHasOrder)
        {
            aiPath.destination = playerDestination;
        }
        else
        {
            // Ngược lại thì trở về vị trí cũ như trước (nếu bạn vẫn muốn)
            aiPath.destination = oldPosition;
        }
    }
    public void IssuePlayerMoveOrder(Vector3 dest)
    {
        playerHasOrder = true;
        playerDestination = dest;
        aiPath.destination = playerDestination;
    }

    private bool Reached(Vector3 a, Vector3 b, float dist) =>
        Vector2.Distance(a, b) <= dist;

    void Update()
    {
        // 1. Nếu có lệnh người chơi → chỉ chạy đến playerDestination
        if (playerHasOrder)
        {
            Debug.Log("Đang chạy tới lệnh người chơi: " + playerDestination);
            aiPath.destination = playerDestination;
            if(oldPosition!=null&&oldPosition!=playerDestination)
            {
                Debug.Log("Cập nhật oldPosition");
                oldPosition =playerDestination; // Cập nhật oldPosition để trở về sau này
            }
            if (Reached(transform.position, playerDestination, arrivalThreshold))
            {
                Debug.Log("Xong order of player");
                playerHasOrder = false;
            }
            return; // Bỏ qua logic AI khác
        }

        // 2. Không có lệnh → chạy AI như cũ
        // chay di nap bom
        if (!bomAnimation.isHoldingBom && !bomAnimation.isThrowing)
        {
            if(gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (!storedOldPosition)
                {
                    oldPosition = transform.position;
                    storedOldPosition = true;
                }
                Transform nearestEnemyStore = PoolBomBullet.Instance.GetNearestEnemyBomStore(transform.position);
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
                Transform nearestPlyerStore = PoolBomBullet.Instance.GetNearestBomStore(transform.position);
                if (nearestPlyerStore != null)
                {
                    aiPath.destination = nearestPlyerStore.position;
                }
                return;
            }
            
        }

        if (atkCheck.EnermyInSafeRange && atkCheck.closestEnemy != null)
        {
            Vector2 retreatDir = ((Vector2)transform.position - (Vector2)atkCheck.closestEnemy.position).normalized;
            Vector2 retreatPos = (Vector2)transform.position + retreatDir * (atkCheck.attackRange + atkCheck.safeRange);
            aiPath.destination = retreatPos;
            return;
        }

        if (bomAnimation.isThrowing)
        {
            aiPath.destination = transform.position;
            return;
        }

        if (bomAnimation.isHoldingBom && atkCheck.closestEnemy != null && atkCheck.EnermyInViewRange)
        {
            aiPath.destination = atkCheck.closestEnemy.position;
            return;
        }
        //khi khong co ke dich hoac nguoi choi yeu cau
        else
        {
            //neu la enemy
            if(gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if(EnemyTarget1!=null)
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

    private void Patrol()
    {
        atkCheck.PatrolWhenNoEnemy();
    }
}
