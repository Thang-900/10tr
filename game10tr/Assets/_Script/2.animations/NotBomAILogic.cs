using UnityEngine;
using Pathfinding;

public class AILogic : MonoBehaviour
{
    private AIMoveToSafeAtkCheckRange aIMoveToSafeAtkCheckRange;
    private Animator animator;
    private AIPath aiPath;
    private Transform characterTransform;
    private SpriteRenderer spriteRenderer;

    private GameObject handUp, handDown, handSide;
    private GameObject idlingUp, idlingDown, idlingSide;
    private GameObject atkUp, atkDown, atkSide;

    private Vector2 lastMoveDir = Vector2.down;
    public bool isAttacking = false;

    public CharacterStats characterStats;
    private BulletOnEven bulletOnEven;

    private enum Direction { Up, Down, Side }

    void Start()
    {
        aIMoveToSafeAtkCheckRange = GetComponent<AIMoveToSafeAtkCheckRange>();
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        characterTransform = transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        bulletOnEven = GetComponentInChildren<BulletOnEven>();

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


    public void IdleAndMove(Vector2 velocity)
    {
        
        if (Vector3.Distance(transform.position, aiPath.destination) > 0.1f)
        {
            ResetATK();
            ResetIdle();
            ResetHands();
            animator.enabled = true;
            spriteRenderer.enabled = true;
            animator.SetFloat("moveX", velocity.x);
            animator.SetFloat("moveY", velocity.y);
            lastMoveDir = velocity.normalized;

            // Flip character based on direction
            if (velocity.x < -0.1f)
                characterTransform.localScale = new Vector3(1, 1, 1);
            else if (velocity.x > 0.1f)
                characterTransform.localScale = new Vector3(-1, 1, 1);

            SetHandActive(velocity);
        }
        else
        {
            ResetIdle();
            ResetHands();
            animator.enabled = false;
            spriteRenderer.enabled = false; 
            SetIdleActive(velocity);
        }
    }
    //sua lai
    public void StartAttack(Vector2 lastMoveDirection)
    {
        isAttacking = true;
        Direction dir = GetDirection(lastMoveDirection);
        SetATKActive(lastMoveDirection);
        Debug.Log("Bắt đầu tấn công");
    }
    //sua lai
    public void EndAttack()
    {
        Debug.Log("Kết thúc animation đánh");
        isAttacking = false;

        if (aIMoveToSafeAtkCheckRange.EnermyInAttackRange)
        {
            SetATKActive(lastMoveDir);  // Chỉ gọi sau khi animation kết thúc
        }
    }

    public void SetHandActive(Vector2 lastMoveDirection)
    {
        ResetHands(); // Reset all hand animations first
        Direction dir = GetDirection(lastMoveDirection);
        if (dir == Direction.Up) handUp.SetActive(true);
        else if (dir == Direction.Down) handDown.SetActive(true);
        else handSide.SetActive(true);
    }

    public void SetIdleActive(Vector2 lastMoveDirection)
    {
        ResetIdle(); // Reset all idle animations first
        Direction dir = GetDirection(lastMoveDirection);
        if (dir == Direction.Up) idlingUp.SetActive(true);
        else if (dir == Direction.Down) idlingDown.SetActive(true);
        else idlingSide.SetActive(true);
    }

    public void SetATKActive(Vector2 lastMoveDirection)
    {
        ResetATK(); // Reset all attack animations first
        Direction dir = GetDirection(lastMoveDirection);
        if (dir == Direction.Up) atkUp.SetActive(true);
        else if (dir == Direction.Down) atkDown.SetActive(true);
        else atkSide.SetActive(true);
    }


    // kiem tra huong hien tai
    private Direction GetDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
            return dir.y > 0 ? Direction.Up : Direction.Down;
        else
            return Direction.Side;
    }

    public void ResetATK()
    {
        atkUp.SetActive(false); atkDown.SetActive(false); atkSide.SetActive(false);
    }

    public void ResetIdle()
    {
        idlingUp.SetActive(false); idlingDown.SetActive(false); idlingSide.SetActive(false);
    }

    public void ResetHands()
    {
        handUp.SetActive(false); handDown.SetActive(false); handSide.SetActive(false);
    }


}
