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
        ResetHands();
        ResetIdle();
        ResetATK();
    }

    void Update()
    {
        Vector2 velocity = aiPath.velocity;
        if (aIMoveToSafeAtkCheckRange.isAtkingEnermy)
        {
            
            GetComponent<SpriteRenderer>().enabled = false;
            Debug.Log("dang danh enemy");
            SetATKActive();
            setATK.isATK = true;
            if (!setATK.isATK)
            {
                Debug.Log("het danh enemy");
                return;
            }
            return;
        }

        GetComponent<SpriteRenderer>().enabled = true;
        if (Vector3.Distance(transform.position, aiPath.destination) > 0.1f)
        {
            Debug.Log("dang di chuyen");
            animator.SetFloat("moveX", velocity.x);
            animator.SetFloat("moveY", velocity.y);
            lastMoveDir = velocity.normalized;

            if (velocity.x < -0.1f)
                characterTransform.localScale = new Vector3(1, 1, 1);
            else if (velocity.x > 0.1f)
                characterTransform.localScale = new Vector3(-1, 1, 1);

            if (!aIMoveToSafeAtkCheckRange.isAtkingEnermy)
            {
                SetHandActive();
            }
            
        }
        else if (Vector3.Distance(transform.position, aiPath.destination) < 0.1f && !aIMoveToSafeAtkCheckRange.isAtkingEnermy)
        {
            // Đã đến đích, xử lý như đứng yên
            Debug.Log("da den dich");
            
            SetIdleActive();
        }


    }

    void SetHandActive()
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

    void SetIdleActive()
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
    void SetATKActive()
    {
        ResetHands();
        ResetIdle();
        GetComponent<SpriteRenderer>().enabled = false;
        if (Mathf.Abs(lastMoveDir.y) > Mathf.Abs(lastMoveDir.x))
        {
            if (lastMoveDir.y > 0)
            {
                atkUp.SetActive(true);
                setATK = atkUp.GetComponent<setATK>();
                atkDown.SetActive(false);
                atkSide.SetActive(false);

            }

            else
            {
                atkUp.SetActive(false);
                atkDown.SetActive(true);
                setATK = atkDown.GetComponent<setATK>();
                atkSide.SetActive(false);

            }
        }
        else
        {
            atkUp.SetActive(false);
            atkDown.SetActive(false);
            atkSide.SetActive(true);
            setATK = atkSide.GetComponent<setATK>();
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
}

