using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent(typeof(AIPath))]
public class BomAnimation : MonoBehaviour
{
    private AIMoveToSafeAtkCheckRange atkCheck;
    private Animator animator;
    private AIPath aiPath;
    private Transform characterTransform;
    //private Index index;
    private AIMoveToSafeAtkCheckRange aiMoveToSafeAtkCheckRange;
    private BomAIPositions bomAIPositions;

    private GameObject handUp;
    private GameObject handDown;
    private GameObject handRight;
    private float satThuonCuaBom;

    public bool isHoldingBom = false;
    public bool isThrowing = false;

    //chuyển sang phần xử lý di chuyển
    //private bool isSearchingForBom = false;
    private Vector2 lastMoveDir = Vector2.down; // hướng mặc định ban đầu
    private Vector3 oldPosition;

    void Start()
    {
        atkCheck = GetComponent<AIMoveToSafeAtkCheckRange>();
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        //index = GetComponent<Index>();
        aiMoveToSafeAtkCheckRange = GetComponent<AIMoveToSafeAtkCheckRange>();
        bomAIPositions = GetComponent<BomAIPositions>();
        characterTransform = transform;

        handUp = transform.Find("hand_Up").gameObject;
        handDown = transform.Find("hand_Down").gameObject;
        handRight = transform.Find("hand_Side").gameObject;
        isHoldingBom = true;
        animator.SetBool("isHoldingBom", true);
    }

    void Update()
    {
        if(animator.GetBool("isThrowing")!=isThrowing)
        {
            animator.SetBool("isThrowing", isThrowing);
        }
        if(animator.GetBool("isHoldingBom") != isHoldingBom)
        {
            animator.SetBool("isHoldingBom", isHoldingBom);
        }
        //đang ném bom
        if (isThrowing)
        {
            animator.SetFloat("speed", 0f);
            return;
        }
        //đang di chuyển hoặc đứng yên
        Vector2 velocity = aiPath.velocity;
        animator.SetFloat("speed", velocity.magnitude);

        if (velocity.sqrMagnitude > 0.01f)
        {
            lastMoveDir = velocity.normalized; // lưu hướng di chuyển cuối
            animator.SetFloat("moveX", velocity.x);
            animator.SetFloat("moveY", velocity.y);

            if (velocity.x < -0.1f)
                characterTransform.localScale = new Vector3(1, 1, 1);
            else if (velocity.x > 0.1f)
                characterTransform.localScale = new Vector3(-1, 1, 1);
        }
        //dang giữ bom
        if (isHoldingBom)
        {
            UpdateHand(velocity.sqrMagnitude > 0.01f ? velocity : lastMoveDir);

            if (atkCheck.EnermyInAttackRange && !atkCheck.EnermyInSafeRange)
            {
                AIBomAtk();
            }
        }
        //không có bom, tắt bom ở tay
        else
        {
            DisableAllHands();
        }
    }

    void AIBomAtk()
    {
        if (!isHoldingBom || isThrowing|| bomAIPositions.playerHasOrder) return;
        DisableAllHands();
        Debug.Log("Gọi AIBomAtk()");
        isThrowing = true;
        isHoldingBom = false;

        //aiPath.destination = aiPath.position;
        //aiPath.canMove = false;

        animator.SetBool("isThrowing", true);
        animator.SetBool("isHoldingBom", false);

        //chờ ném bom xong mới tiếp tục di chuyển
        StartCoroutine(ResumeAfterThrow());

        //Ném bom tới vị trí của kẻ địch gần nhất
        Vector3 targetPos = atkCheck.closestEnemy.position;
        ThrowBomb(targetPos);
    }

    void ThrowBomb(Vector3 targetPosition)
    {
        GameObject bomb = PoolBomBullet.Instance.GetBomb();
        bomb.transform.position = transform.position;
        Bomb bombScript = bomb.GetComponent<Bomb>();
        bombScript.ThrowTo(targetPosition);
        bombScript.enemyTag = aiMoveToSafeAtkCheckRange.targetLayer;
    }

    IEnumerator ResumeAfterThrow()
    {
        yield return new WaitForSeconds(1.8f);

        isThrowing = false;

        animator.SetBool("isThrowing", false);
        atkCheck.ResetTarget();

        Debug.Log("Xong animation, tiếp tục di chuyển.");
    }

    void UpdateHand(Vector2 velocity)
    {
        DisableAllHands();

        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            handRight.SetActive(true);
        }
        else
        {
            if (velocity.y > 0)
                handUp.SetActive(true);
            else
                handDown.SetActive(true);
        }
    }

    void DisableAllHands()
    {
        handUp.SetActive(false);
        handDown.SetActive(false);
        handRight.SetActive(false);
    }
   
    public bool IsHoldingBom() => isHoldingBom;

    public void SetHoldingBom(bool value)
    {
        isHoldingBom = value;
        animator.SetBool("isHoldingBom", value);
    }

    public bool IsThrowing() => isThrowing;

    public void SetThrowing(bool value)
    {
        isThrowing = value;
        animator.SetBool("isThrowing", value);
    }
}
