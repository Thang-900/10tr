// transitions.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitions : MonoBehaviour
{
    [SerializeField] private GameObject[] bom; // Mảng chứa các bom
    public Animator animator;
    public bool isHoldingBom = false;
    float vertical;
    float horizontal;
    float directionX;
    float directionY;
    public GameObject sideBom;
    public GameObject upBom;
    public GameObject downBom;
    public BomMove bomMove; // Kéo vào Inspector

    private void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");

        // 1) Nếu đang cầm bom và **chưa** bắt đầu ném
        if (isHoldingBom && !bomMove.isThrowing)
        {
            if (vertical != 0 || horizontal != 0)
            {
                directionX = horizontal;
                directionY = vertical;

                // Hiện bom theo hướng
                sideBom.SetActive((Mathf.Abs(directionX) == 1 && directionY == 0));
                upBom.SetActive(directionY == 1);
                downBom.SetActive(directionY == -1);

                animator.SetBool("isHoldingBomAndWalking", true);
                animator.SetBool("isHoldingBomAndIdling", false);
            }
            else
            {
                // Đứng yên giữ hướng cuối cùng
                sideBom.SetActive((Mathf.Abs(directionX) == 1 && directionY == 0));
                upBom.SetActive(directionY == 1);
                downBom.SetActive(directionY == -1);

                animator.SetBool("isHoldingBomAndWalking", false);
                animator.SetBool("isHoldingBomAndIdling", true);
            }

            // Reset các state không liên quan
            animator.SetBool("isWalking", false);
            animator.SetBool("isIdling", false);
            animator.SetFloat("horizontal", directionX);
            animator.SetFloat("vertical", directionY);
        }
        //// 2) Nếu bom đang bay (đang ném) → bỏ qua hoàn toàn, không đổi animation
        //else if (bomMove.isThrowing)
        //{
        //    // Không làm gì, giữ nguyên animation cũ cho đến khi bomMove.isThrowing = false
        //}
        // 3) Không cầm bom và bom cũng không bay
        else
        {
            // Tắt hết sprite bom
            foreach (GameObject b in bom)
                b.SetActive(false);
            animator.SetBool("isHoldingBomAndWalking", false);
            animator.SetBool("isHoldingBomAndIdling", false);
            if (vertical != 0 || horizontal != 0)
            {
                directionX = horizontal;
                directionY = vertical;
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdling", false);
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", true);
            }

            // Reset các state cầm bom
            animator.SetBool("isHoldingBomAndWalking", false);
            animator.SetBool("isHoldingBomAndIdling", false);
            animator.SetFloat("horizontal", directionX);
            animator.SetFloat("vertical", directionY);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bom") && !isHoldingBom)
        {
            isHoldingBom = true;
            Destroy(other.gameObject);
        }
    }
}
