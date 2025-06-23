using UnityEngine;

public class transitions : MonoBehaviour
{
    [SerializeField] private GameObject[] bom; // Mảng chứa các bom
    public Animator animator;
    public bool isHoldingBom = false;
    public bool isWeaponing = false;
    //bool isHoldingBomAndWalking = false;
    //bool isHoldingBomAndIdling = false;
    float vertical;
    float horizontal;
    float directionX;
    float directionY;
    public GameObject sideBom;
    public GameObject upBom;
    public GameObject downBom;
    public BomMove bomMove; // Biến để truy cập vào script BomMove

    
    private void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        if (isHoldingBom && !bomMove.isThrowing && isWeaponing)
        {
            
            if (vertical != 0 || horizontal != 0)
            {
                Debug.Log("dang cam bom va di chuyen");
                directionX = horizontal;
                directionY = vertical;
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
            animator.SetFloat("horizontal", directionX);
            animator.SetFloat("vertical", directionY);
        }


        else
        {
            foreach (GameObject b in bom)
            {
                b.SetActive(false);
            }
            animator.SetBool("isHoldingBomAndWalking", false);
            animator.SetBool("isHoldingBomAndIdling", false);
            if (vertical != 0 || horizontal != 0)
            {
                Debug.Log("dang di chuyen");
                directionX = horizontal;
                directionY = vertical;

                animator.SetBool("isWalking", true);
                animator.SetBool("isIdling", false);
            }
            else
            {
                Debug.Log("dang dung yen");
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
            isWeaponing = true; // Đặt trạng thái đang sử dụng vũ khí
            Debug.Log("Bom da duoc nhat");
            isHoldingBom = true;
            Destroy(other.gameObject); // Hủy bom khi nhặt được
        }
    }

}