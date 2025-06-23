using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmerAnimation : MonoBehaviour
{
    



    private void HarmerAnimations(bool isHoldingHarmer, bool isWeaponing, float x, float y,Animator animator, GameObject sideHarmer, GameObject upHarmer, GameObject downHarmer)
    {
        
        if (isHoldingHarmer && isWeaponing)
        {
            
            if (x != 0 || y != 0)
            {
                Debug.Log("dang cam bua va di chuyen");
                
                //hien bua di sang hai ben
                if ((x == 1 || x == -1) && y == 0)
                {
                    sideHarmer.SetActive(true);
                    upHarmer.SetActive(false);
                    downHarmer.SetActive(false);
                }
                //directionX == 0 ||directionX==1||directionX==-1 && 
                //hien bua dang di len
                else if (y == 1)
                {
                    sideHarmer.SetActive(false);
                    upHarmer.SetActive(true);
                    downHarmer.SetActive(false);
                }
                //directionX == 0 && 
                //hien bua dang di xuong
                else if (y == -1)
                {
                    sideHarmer.SetActive(false);
                    upHarmer.SetActive(false);
                    downHarmer.SetActive(true);
                }

                animator.SetBool("isWalking", true);
                animator.SetBool("isIdling", false);
            }
            else
            {
                Debug.Log("dang cam bua va dung yen");
                if ((x == 1 || x == -1) && y == 0)
                {
                    sideHarmer.SetActive(true);
                    upHarmer.SetActive(false);
                    downHarmer.SetActive(false);
                }
                //directionX == 0 ||directionX==1||directionX==-1 && 
                //hien bom dang di len
                else if (y == 1)
                {
                    sideHarmer.SetActive(false);
                    upHarmer.SetActive(true);
                    downHarmer.SetActive(false);
                }
                //directionX == 0 && 
                //hien bom dang di xuong
                else if (y == -1)
                {
                    sideHarmer.SetActive(false);
                    upHarmer.SetActive(false);
                    downHarmer.SetActive(true);
                }

                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", true);
            }
        }
        
        animator.SetFloat("horizontal", x);
        animator.SetFloat("vertical", y);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Harmer") )
        {
            Debug.Log("bua da duoc nhat");
            Destroy(other.gameObject); // Hủy bua khi nhặt được
        }
    }

}
