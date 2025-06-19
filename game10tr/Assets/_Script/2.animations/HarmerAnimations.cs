using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmerAnimations : MonoBehaviour
{
    [SerializeField] private GameObject[] harmer; // Mảng chứa các bua
    public transitions transitions; // Biến để truy cập vào script transitions
    public Animator animator;
    public bool isHoldingHarmer = false;
    //bool isHoldingBomAndWalking = false;
    //bool isHoldingBomAndIdling = false;
    float vertical;
    float horizontal;
    float directionX;
    float directionY;
    public GameObject sideHarmer;
    public GameObject upHarmer;
    public GameObject downHarmer;



    private void Update()
    {
        
        if (isHoldingHarmer && transitions.isWeaponing)
        {
            vertical = Input.GetAxisRaw("Vertical");
            horizontal = Input.GetAxisRaw("Horizontal");
            if (vertical != 0 || horizontal != 0)
            {
                Debug.Log("dang cam bua va di chuyen");
                directionX = horizontal;
                directionY = vertical;
                //hien bua di sang hai ben
                if ((directionX == 1 || directionX == -1) && directionY == 0)
                {
                    sideHarmer.SetActive(true);
                    upHarmer.SetActive(false);
                    downHarmer.SetActive(false);
                }
                //directionX == 0 ||directionX==1||directionX==-1 && 
                //hien bua dang di len
                else if (directionY == 1)
                {
                    sideHarmer.SetActive(false);
                    upHarmer.SetActive(true);
                    downHarmer.SetActive(false);
                }
                //directionX == 0 && 
                //hien bua dang di xuong
                else if (directionY == -1)
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
                if ((directionX == 1 || directionX == -1) && directionY == 0)
                {
                    sideHarmer.SetActive(true);
                    upHarmer.SetActive(false);
                    downHarmer.SetActive(false);
                }
                //directionX == 0 ||directionX==1||directionX==-1 && 
                //hien bom dang di len
                else if (directionY == 1)
                {
                    sideHarmer.SetActive(false);
                    upHarmer.SetActive(true);
                    downHarmer.SetActive(false);
                }
                //directionX == 0 && 
                //hien bom dang di xuong
                else if (directionY == -1)
                {
                    sideHarmer.SetActive(false);
                    upHarmer.SetActive(false);
                    downHarmer.SetActive(true);
                }

                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", true);
            }
        }
        
        animator.SetFloat("horizontal", directionX);
        animator.SetFloat("vertical", directionY);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Harmer") && !transitions.isWeaponing)
        {
            Debug.Log("bua da duoc nhat");
            isHoldingHarmer = true; // Đặt trạng thái đang cầm bua
            transitions.isWeaponing = true;
            Destroy(other.gameObject); // Hủy bua khi nhặt được
        }
    }

}
