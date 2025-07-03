using Pathfinding;
using UnityEngine;

public class AIHammer : MonoBehaviour
{
    private AIMoveToSafeAtkCheckRange aIMoveToSafeAtkCheckRange;
    private Animator animator;
    private AIPath aiPath;
    private Transform characterTransform;
    private setATK setATK;
    private GameObject handUp, handDown, handSide;
    private GameObject idlingUp, idlingDown, idlingSide;
    private GameObject atkUp, atkDown, atkSide;

    private Vector2 lastMoveDir = Vector2.down;

    void Start()
    {
        aIMoveToSafeAtkCheckRange = GetComponent<AIMoveToSafeAtkCheckRange>();
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        characterTransform = transform;

        handUp = transform.Find("Hand_Up").gameObject;
        handDown = transform.Find("Hand_Down").gameObject;
        handSide = transform.Find("Hand_Side").gameObject;

        idlingUp = transform.Find("idleHammerUp").gameObject;
        idlingDown = transform.Find("idleHammerDown").gameObject;
        idlingSide = transform.Find("idleHammerSide").gameObject;

        atkUp = transform.Find("hammer_attack_up").gameObject;
        atkDown = transform.Find("hammer_attack_down").gameObject;
        atkSide = transform.Find("hammer_attack_side").gameObject;

        if (handUp == null || handDown == null || handSide == null ||
            idlingUp == null || idlingDown == null || idlingSide == null)
        {
            Debug.LogError("One or more hand objects are not assigned in the AIHammer script.");
        }
    }

    void Update()
    {
        Vector2 velocity = aiPath.velocity;
        //if (aIMoveToSafeAtkCheckRange.isAtkingEnermy)
        //{
        //    animator.SetBool("isMoving", false);
        //    SetATKActive();
        //    GetComponent<SpriteRenderer>().enabled = false;
        //    if (!setATK.isATK)
        //    {
        //        return;
        //    }
        //}

        GetComponent<SpriteRenderer>().enabled = true;
        if (Vector3.Distance(transform.position, aiPath.destination) > 0.1f)
        {
            Debug.Log("dang di chuyen");

            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", velocity.x);
            animator.SetFloat("moveY", velocity.y);
            lastMoveDir = velocity.normalized;

            if (velocity.x < -0.1f)
                characterTransform.localScale = new Vector3(1, 1, 1);
            else if (velocity.x > 0.1f)
                characterTransform.localScale = new Vector3(-1, 1, 1);

            SetHandActive();
        }
        else if (Vector3.Distance(transform.position, aiPath.destination) < 0.1f)
        {
            // Đã đến đích, xử lý như đứng yên
            Debug.Log("da den dich");
            animator.SetBool("isMoving", false);
            SetIdleActive();
        }


    }

    void SetHandActive()
    {
        atkUp.SetActive(false);
        atkDown.SetActive(false);
        atkSide.SetActive(false);
        handUp.SetActive(false);
        handDown.SetActive(false);
        handSide.SetActive(false);
        idlingUp.SetActive(false);
        idlingDown.SetActive(false);
        idlingSide.SetActive(false);
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

    void SetIdleActive()
    {
        atkUp.SetActive(false);
        atkDown.SetActive(false);
        atkSide.SetActive(false);
        handUp.SetActive(false);
        handDown.SetActive(false);
        handSide.SetActive(false);
        idlingUp.SetActive(false);
        idlingDown.SetActive(false);
        idlingSide.SetActive(false);

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
    void SetATKActive()
    {
        atkUp.SetActive(false);
        atkDown.SetActive(false);
        atkSide.SetActive(false);
        handUp.SetActive(false);
        handDown.SetActive(false);
        handSide.SetActive(false);
        idlingUp.SetActive(false);
        idlingDown.SetActive(false);
        idlingSide.SetActive(false);

        GetComponent<SpriteRenderer>().enabled = false;
        if (Mathf.Abs(lastMoveDir.y) > Mathf.Abs(lastMoveDir.x))
        {
            if (lastMoveDir.y > 0)
                atkUp.SetActive(true);
            else
                handDown.SetActive(true);
        }
        else
        {
            atkSide.SetActive(true);
        }
    }
}
