using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitions : MonoBehaviour
{
    public Animator animator;
    bool isHoldingBom = false;
    bool isHoldingBomAndWalking = false;
    bool isHoldingBomAndIdling = false;
    float vertical;
    float horizontal;
    float directionX;
    float directionY;
    public GameObject sideBom;
    public GameObject upBom;
    public GameObject downBom;

    private void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        if (isHoldingBom)
        {
            if (vertical != 0 || horizontal != 0)
            {
                Debug.Log("chuyen sang dung yen");
                directionX = horizontal;
                directionY = vertical;
                //hien bom di sang hai ben
                if ((directionX == 1 || directionX == -1) && directionY == 0)
                {
                    sideBom.SetActive(true);
                    upBom.SetActive(false);
                    downBom.SetActive(false);
                }
                //directionX == 0 ||directionX==1||directionX==-1 && 
                //hien bom dang di len
                else if (directionY == 1)
                {
                    sideBom.SetActive(false);
                    upBom.SetActive(true);
                    downBom.SetActive(false);
                }
                //directionX == 0 && 
                //hien bom dang di xuong
                else if (directionY == -1)
                {
                    upBom.SetActive(false);
                    sideBom.SetActive(false);
                    downBom.SetActive(true);
                }



                animator.SetBool("isHoldingBomAndWalking", true);
                animator.SetBool("isHoldingBomAndIdling", false);
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", false);
                
            }
            else
            {
                Debug.Log("chuyen sang dung yen");
                if ((directionX == 1 || directionX == -1) && directionY == 0)
                {
                    sideBom.SetActive(true);
                    upBom.SetActive(false);
                    downBom.SetActive(false);
                }
                //directionX == 0 ||directionX==1||directionX==-1 && 
                //hien bom dang di len
                else if (directionY == 1)
                {
                    sideBom.SetActive(false);
                    upBom.SetActive(true);
                    downBom.SetActive(false);
                }
                //directionX == 0 && 
                //hien bom dang di xuong
                else if (directionY == -1)
                {
                    upBom.SetActive(false);
                    sideBom.SetActive(false);
                    downBom.SetActive(true);
                }
                animator.SetBool("isHoldingBomAndWalking", false);
                animator.SetBool("isHoldingBomAndIdling", true);
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", false);
            }
            animator.SetFloat("horizontal", directionX);
            animator.SetFloat("vertical", directionY);
        }


        else
        {
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

            animator.SetFloat("horizontal", directionX);
            animator.SetFloat("vertical", directionY);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bom") && !isHoldingBom)
        {
            isHoldingBom = true;
            other.gameObject.SetActive(false);
        }
    }

}
