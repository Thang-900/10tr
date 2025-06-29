using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent(typeof(AIPath))]
public class AIAnimationController : MonoBehaviour
{
    private AIMoveToSafeAtkCheckRange aIMoveToSafeAtkCheckRange;
    private Animator animator;
    private AIPath aiPath;
    private Transform characterTransform;

    private GameObject handUp;
    private GameObject handDown;
    private GameObject handRight;

    private bool isHoldingBom = false;
    private bool isThrowing = false;

    void Start()
    {
        aIMoveToSafeAtkCheckRange = GetComponent<AIMoveToSafeAtkCheckRange>();
        aiPath = GetComponent<AIPath>();
        animator = GetComponent<Animator>();
        characterTransform = transform;

        handUp = transform.Find("Hand_Up").gameObject;
        handDown = transform.Find("Hand_Down").gameObject;
        handRight = transform.Find("Hand_Side").gameObject;

        isHoldingBom = true;
        animator.SetBool("isHoldingBom", true);
    }

    void Update()
    {
        if (isThrowing)
        {
            animator.SetFloat("speed", 0f); // Chặn chuyển animation khác
            return;
        }

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

            if (isHoldingBom)
                UpdateHand(velocity);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            AIBomAtk();
        }
    }

    void AIBomAtk()
    {
        //if (!isHoldingBom || isThrowing) return;

        Debug.Log("Goi ham AIBomAtk()");
        isThrowing = true;
        isHoldingBom = false;

        aiPath.destination = aiPath.position; // Ngắt di chuyển

        animator.SetBool("isThrowing", true);
        animator.SetBool("isHoldingBom", false); 

        StartCoroutine(ResumeAfterThrow());
    }

    IEnumerator ResumeAfterThrow()
    {
        yield return new WaitForSeconds(1.8f);

        isThrowing = false;
        aiPath.canMove = true;

        animator.SetBool("isThrowing", false);
        animator.SetBool("isHoldingBom", false);

        Debug.Log("Da xong animation, co the di chuyen lai.");
    }

    void UpdateHand(Vector2 velocity)
    {
        handUp.SetActive(false);
        handDown.SetActive(false);
        handRight.SetActive(false);

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
}
