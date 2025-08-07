using Pathfinding;
using UnityEngine;

public class AIGun : MonoBehaviour
{
    private AILogic aiLogic;
    private AIPath aiPath;
    private AIMoveToSafeAtkCheckRange aIMoveToSafeAtkCheckRange;
    private Vector2 lastMoveDir = Vector2.down;
    private bool isRecharingBullet = false;
    private Vector2 oldPosition;
    private Vector2 direction;

    public BulletOnEven bulletOnEvenUp;
    public BulletOnEven bulletOnEvenDown;
    public BulletOnEven bulletOnEvenSide;

    private int bulletCount = 0;
    public int maxBulletCount = 10;

    void Start()
    {
        aiLogic = GetComponent<AILogic>();
        aiPath = GetComponent<AIPath>();
        aIMoveToSafeAtkCheckRange = GetComponent<AIMoveToSafeAtkCheckRange>();
        bulletCount = 0;
    }

    void Update()
    {
        bulletCount = bulletOnEvenUp.currentBulletCount + bulletOnEvenDown.currentBulletCount + bulletOnEvenSide.currentBulletCount;
        if(aiPath.velocity.magnitude>0)
        {
            direction = aiPath.velocity.normalized;
        }
        if (bulletCount >= maxBulletCount && !isRecharingBullet)
        {
            // Hết đạn → đi nạp
            oldPosition = transform.position;
            aiPath.destination = PoolBomBullet.Instance.GetNearestBulletStore(transform.position).position;
            isRecharingBullet = true;
            aIMoveToSafeAtkCheckRange.enabled = false;
        }
        else if (aIMoveToSafeAtkCheckRange.EnermyInAttackRange && bulletCount < maxBulletCount)
        {
            // Có enemy và còn đạn → chiến đấu
            isRecharingBullet = false;
            aIMoveToSafeAtkCheckRange.enabled = true;
            aiPath.destination = transform.position;
            if(!aiLogic.isAttacking)
            {
                aiLogic.ResetATK();
                aiLogic.SetATKActive(direction);
            }
            
        }

        aiLogic.IdleAndMove(aiPath.velocity);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("BulletStore") && isRecharingBullet)
        {
            // Nạp lại đạn thành công
            isRecharingBullet = false;
            bulletOnEvenUp.ReloadBulletOnEven();
            bulletOnEvenDown.ReloadBulletOnEven();
            bulletOnEvenSide.ReloadBulletOnEven();
            bulletCount = 0;
            aIMoveToSafeAtkCheckRange.enabled = true;
            aiPath.destination = oldPosition;
        }
    }
}
