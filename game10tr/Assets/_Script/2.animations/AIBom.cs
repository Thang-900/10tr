using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent(typeof(AIPath))]
public class AIBom : MonoBehaviour
{
    private AIMoveToSafeAtkCheckRange atkCheck;
    private Animator animator;
    private AIPath aiPath;
    private Transform characterTransform;

    private GameObject handUp;
    private GameObject handDown;
    private GameObject handRight;

    private bool isHoldingBom = false;
    private bool isThrowing = false;
    private bool isSearchingForBom = false;

    private Vector3 oldPosition;

    void Start()
    {
        atkCheck = GetComponent<AIMoveToSafeAtkCheckRange>();
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        characterTransform = transform;

        handUp = transform.Find("hand_Up").gameObject;
        handDown = transform.Find("hand_Down").gameObject;
        handRight = transform.Find("hand_Side").gameObject;

        isHoldingBom = true;
        animator.SetBool("isHoldingBom", true);
    }

    void Update()
    {
        atkCheck.EngageWithEnemy(); // Gọi mỗi frame để cập nhật trạng thái enemy
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
            UpdateHand(velocity);

            if (atkCheck.EnermyInAttackRange && !atkCheck.EnermyInSafeRange)
            {
                DisableAllHands();
                AIBomAtk();
            }
        }
        else
        {
            DisableAllHands();
            atkCheck.enabled = false;

            if (!isSearchingForBom)
            {
                isSearchingForBom = true;
                Transform nearestStore = PoolBomBullet.Instance.GetNearestBomStore(transform.position);
                if (nearestStore != null)
                {
                    aiPath.destination = nearestStore.position;
                }
            }
        }
    }

    void AIBomAtk()
    {
        if (!isHoldingBom || isThrowing) return;

        Debug.Log("Gọi AIBomAtk()");
        isThrowing = true;
        isHoldingBom = false;

        aiPath.destination = aiPath.position;
        aiPath.canMove = false;

        animator.SetBool("isThrowing", true);
        animator.SetBool("isHoldingBom", false);

        StartCoroutine(ResumeAfterThrow());

        Vector3 targetPos = atkCheck.closestEnemy.position;
        ThrowBomb(targetPos);
    }

    void ThrowBomb(Vector3 targetPosition)
    {
        GameObject bomb = PoolBomBullet.Instance.GetBomb();
        bomb.transform.position = transform.position;
        Bomb bombScript = bomb.GetComponent<Bomb>();
        bombScript.ThrowTo(targetPosition);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHoldingBom && collision.CompareTag("BomStore"))
        {
            Debug.Log("Nhặt được bom mới.");
            isHoldingBom = true;
            isSearchingForBom = false;
            animator.SetBool("isHoldingBom", true);
            atkCheck.EnermyInAttackRange = false;
        }
    }

    // Lưu game
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
