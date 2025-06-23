using UnityEngine;

public class BomAnimation : MonoBehaviour
{
    public void BomAnimations(bool isHoldingBom,bool isThrowing, bool isWeaponing,float x, float y, Animator animator, GameObject sideBom, GameObject upBom, GameObject downBom)
    {

        if (isHoldingBom && !isThrowing && isWeaponing)
        {
            if (x != 0 || y != 0)
            {
                Debug.Log("dang cam bom va di chuyen");
                //hien bom di sang hai ben

                animator.SetBool("isHoldingBomAndWalking", true);
                animator.SetBool("isHoldingBomAndIdling", false);
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", false);

            }
            else
            {
                Debug.Log("dang cam bom va dung yen");
                animator.SetBool("isHoldingBomAndWalking", false);
                animator.SetBool("isHoldingBomAndIdling", true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", false);
            }
            if ((x == 1 || x == -1) && y == 0)
            {
                sideBom.SetActive(true);
                upBom.SetActive(false);
                downBom.SetActive(false);
            }
            //directionX == 0 ||directionX==1||directionX==-1 && 
            //hien bom dang di len
            else if (y == 1)
            {
                sideBom.SetActive(false);
                upBom.SetActive(true);
                downBom.SetActive(false);
            }
            //directionX == 0 && 
            //hien bom dang di xuong
            else if (y == -1)
            {
                upBom.SetActive(false);
                sideBom.SetActive(false);
                downBom.SetActive(true);
            }
            animator.SetFloat("Vertical", x);
            animator.SetFloat("Horizontal", y);
        }
        else
        {
            upBom.SetActive(false);
            sideBom.SetActive(false);
            downBom.SetActive(false);
            animator.SetBool("isHoldingBomAndWalking", false);
            animator.SetBool("isHoldingBomAndIdling", false);
        }
    }

    

}