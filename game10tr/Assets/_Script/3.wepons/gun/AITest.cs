using UnityEngine;
using Pathfinding;

public class AIHammer : MonoBehaviour
{
    //dung cho gunner
    public  CharacterStats characterStats;
    public BulletOnEven bulletOnEvenUp;
    public BulletOnEven bulletOnEvenDown;
    public BulletOnEven bulletOnEvenSide;
    private int maxBulletCount = 10;
    private int bulletCount = 0;

    private AIMoveToSafeAtkCheckRange aIMoveToSafeAtkCheckRange;
    private Animator animator;
    private AIPath aiPath;
    private Transform characterTransform;

    private GameObject handUp, handDown, handSide;
    private GameObject idlingUp, idlingDown, idlingSide;
    private GameObject atkUp, atkDown, atkSide;

    private Vector2 lastMoveDir = Vector2.down;
    private bool isAttacking = false;
    private bool isRechargingBullet = false;
    private Vector3 oldDestination;


    void Start()
    {
        aIMoveToSafeAtkCheckRange = GetComponent<AIMoveToSafeAtkCheckRange>();
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        characterTransform = transform;

        handUp = transform.Find("hand_Up").gameObject;
        handDown = transform.Find("hand_Down").gameObject;
        handSide = transform.Find("hand_Side").gameObject;

        idlingUp = transform.Find("idle_Up").gameObject;
        idlingDown = transform.Find("idle_Down").gameObject;
        idlingSide = transform.Find("idle_Side").gameObject;

        atkUp = transform.Find("attack_Up").gameObject;
        atkDown = transform.Find("attack_Down").gameObject;
        atkSide = transform.Find("attack_Side").gameObject;

        ResetHands();
        ResetIdle();
        ResetATK();
    }

    void Update()
    {
        Vector2 velocity = aiPath.velocity;

        if (characterStats.characterType == CharacterType.Gunner)
        {
            // Tính số lượng đạn hiện có
            bulletCount = bulletOnEvenUp.currentBulletCount +
                          bulletOnEvenDown.currentBulletCount +
                          bulletOnEvenSide.currentBulletCount;

            // Đang di chuyển thì lưu hướng
            if (aiPath.velocity.magnitude > 0.1f)
                lastMoveDir = aiPath.velocity.normalized;

            if (bulletCount >= maxBulletCount)
            {
                // Đã đầy đạn → trở lại vị trí cũ
                if (isRechargingBullet)
                {
                    isRechargingBullet = false;
                    aIMoveToSafeAtkCheckRange.enabled = true;
                    aiPath.destination = oldDestination;
                }
            }
            else if (bulletCount < maxBulletCount && !isRechargingBullet)
            {
                // Đạn chưa đầy → đi nạp
                oldDestination = transform.position;
                aiPath.destination = PoolBomBullet.Instance.GetNearestBulletStore(transform.position).position;
                isRechargingBullet = true;
                aIMoveToSafeAtkCheckRange.enabled = false;
            }

            // Nếu đang chiến đấu và chưa bắn
            if (aIMoveToSafeAtkCheckRange.EnermyInAttackRange && !isAttacking && bulletCount < maxBulletCount)
            {
                isRechargingBullet = false;
                aiPath.destination = transform.position; // Đứng yên để bắn
                StartAttack();
            }
        }

        else
        {
            if (!isAttacking && aIMoveToSafeAtkCheckRange.EnermyInAttackRange)
            {
                StartAttack();
            }

            if (!isAttacking && !aIMoveToSafeAtkCheckRange.EnermyInAttackRange)
            {
                GetComponent<SpriteRenderer>().enabled = true;

                if (Vector3.Distance(transform.position, aiPath.destination) > 0.1f)
                {
                    animator.SetFloat("moveX", velocity.x);
                    animator.SetFloat("moveY", velocity.y);
                    lastMoveDir = velocity.normalized;

                    if (velocity.x < -0.1f)
                        characterTransform.localScale = new Vector3(1, 1, 1);
                    else if (velocity.x > 0.1f)
                        characterTransform.localScale = new Vector3(-1, 1, 1);

                    SetHandActive();
                }
                else
                {
                    SetIdleActive();
                }
            }
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        aIMoveToSafeAtkCheckRange.enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        SetATKActive();
        Debug.Log("Bắt đầu tấn công → chờ animation kết thúc");
    }

    public void EndAttack() // gọi từ animation event
    {
        Debug.Log("Kết thúc animation đánh");

        isAttacking = false;
        aIMoveToSafeAtkCheckRange.enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;

        if (!aIMoveToSafeAtkCheckRange.EnermyInAttackRange)
        {
            ResetATK();
            ResetIdle();
            ResetHands();
        }
        else
        {
            StartAttack(); // Tấn công tiếp nếu còn địch
        }
    }

    private void SetHandActive()
    {
        ResetIdle();
        ResetATK();
        ResetHands();
        GetComponent<SpriteRenderer>().enabled = true;

        if (Mathf.Abs(lastMoveDir.y) > Mathf.Abs(lastMoveDir.x))
        {
            if (lastMoveDir.y > 0)
                handUp.SetActive(true);
            else
                handDown.SetActive(true);
        }
        else
        {
            handSide.SetActive(true);
        }
    }

    private void SetIdleActive()
    {
        ResetIdle();
        ResetHands();
        ResetATK();
        GetComponent<SpriteRenderer>().enabled = false;

        if (Mathf.Abs(lastMoveDir.y) > Mathf.Abs(lastMoveDir.x))
        {
            if (lastMoveDir.y > 0)
                idlingUp.SetActive(true);
            else
                idlingDown.SetActive(true);
        }
        else
        {
            idlingSide.SetActive(true);
        }
    }

    private void SetATKActive()
    {
        ResetHands();
        ResetIdle();
        GetComponent<SpriteRenderer>().enabled = false;

        atkUp.SetActive(false);
        atkDown.SetActive(false);
        atkSide.SetActive(false);

        if (Mathf.Abs(lastMoveDir.y) > Mathf.Abs(lastMoveDir.x))
        {
            if (lastMoveDir.y > 0)
            {
                atkUp.SetActive(true);
            }
            else
            {
                atkDown.SetActive(true);
            }
        }
        else
        {
            atkSide.SetActive(true);
        }
    }

    private void ResetATK()
    {
        atkUp.SetActive(false);
        atkDown.SetActive(false);
        atkSide.SetActive(false);
    }

    private void ResetIdle()
    {
        idlingUp.SetActive(false);
        idlingDown.SetActive(false);
        idlingSide.SetActive(false);
    }

    private void ResetHands()
    {
        handUp.SetActive(false);
        handDown.SetActive(false);
        handSide.SetActive(false);
    }
    // Cho script event gọi
    public int GetMaxBulletCount()
    {
        return maxBulletCount;
    }

    public bool HasEnemyInRange()
    {
        return aIMoveToSafeAtkCheckRange != null && aIMoveToSafeAtkCheckRange.EnermyInAttackRange;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletStore") && isRechargingBullet)
        {
            bulletOnEvenUp.ReloadBulletOnEven();
            bulletOnEvenDown.ReloadBulletOnEven();
            bulletOnEvenSide.ReloadBulletOnEven();
            bulletCount = 0;
            Debug.Log("Đã nạp lại đạn xong.");
        }
    }

}
