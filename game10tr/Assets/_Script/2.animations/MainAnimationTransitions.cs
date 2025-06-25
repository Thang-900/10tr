using Unity.VisualScripting;
using UnityEngine;

public class MainAnimationTransitions : MonoBehaviour
{
    public enum WeaponType { None, Bom, Harmer, Gun }

    public Animator animator;
    public GameObject BomUp;
    public GameObject BomDown;
    public GameObject BomSide;
    public GameObject HarmerUp;
    public GameObject HarmerDown;
    public GameObject HarmerSide;
    public GameObject GunUp;
    public GameObject GunDown;
    public GameObject GunSide;
    public GameObject handGunSide;

    private float verticalInput;
    private float horizontalInput;
    private float directionX;
    private float directionY;

    public WeaponType currentWeapon = WeaponType.None;

    private void UpSideDownDirection(float x, float y,GameObject up,GameObject side,GameObject down)
    {
        animator.SetFloat(name: "vertical", y);
        animator.SetFloat("horizontal", x);
        if (up == null || side == null || down == null)
        {
            return;
        }
        else
        {
            if ((x == 1 || x == -1) && y == 0)
            {
                side.SetActive(true);
                up.SetActive(false);
                down.SetActive(false);
            }
            else if (y == 1)
            {
                side.SetActive(false);
                up.SetActive(true);
                down.SetActive(false);
            }
            else if (y == -1)
            {
                up.SetActive(false);
                side.SetActive(false);
                down.SetActive(true);
            }
        }
        
    }

    private void ResetAnimations()
    {
        string[] states = {
            "isWalking", "isIdling",
            "isBomWalking", "isBomIdling",
            "isHarmerWalking", "isHarmerIdling",
            "isGunWalking", "isGunIdling"
        };

        foreach (string state in states)
        {
            animator.SetBool(state, false);
        }
    }

    private void SetAnimations(float vertical, float horizontal, string walk, string idle)
    {
        bool isWalking = vertical != 0 || horizontal != 0;
        animator.SetBool(walk, isWalking);
        animator.SetBool(idle, !isWalking);
    }

    void Update()
    {
        verticalInput = Input.GetAxisRaw("Vertical");//up down
        horizontalInput = Input.GetAxisRaw("Horizontal");//right left

        if (horizontalInput != 0 || verticalInput != 0)
        {
            directionX = horizontalInput;
            directionY = verticalInput;
        }

        ResetAnimations();

        switch (currentWeapon)
        {
            case WeaponType.None:
                SetAnimations(verticalInput, horizontalInput, "isWalking", "isIdling");
                UpSideDownDirection(directionX, directionY, null, null, null);
                break;
            case WeaponType.Bom:
                SetAnimations(verticalInput, horizontalInput, "isBomWalking", "isBomIdling");
                UpSideDownDirection(directionX, directionY,BomUp,BomSide,BomDown);
                break;
            case WeaponType.Harmer:
                SetAnimations(verticalInput, horizontalInput, "isWalking", "isIdling");
                UpSideDownDirection(directionX, directionY, HarmerUp, HarmerSide, HarmerDown);
                break;
            case WeaponType.Gun:
                SetAnimations(verticalInput, horizontalInput, "isGunWalking", "isGunIdling");
                UpSideDownDirection(directionX, directionY, GunUp, GunSide, GunDown);
                if (horizontalInput == 1 || horizontalInput == -1)
                {
                    handGunSide.SetActive(true);
                }
                else
                {
                    handGunSide.SetActive(false);
                }
                break;
        }

        
    }
    private void OnTriggerEnter2D(Collider2D other)

    {
        if (other.CompareTag("Bom"))
        {
            
            SwitchWeapon(WeaponType.Bom);
            Destroy(other.gameObject);
            return;
        }
            
        
        else if (other.CompareTag("Harmer"))
        {
            SwitchWeapon(WeaponType.Harmer);
            Destroy(other.gameObject);
            return;
        }
            

        else if (other.CompareTag("Gun"))
        {
            SwitchWeapon(WeaponType.Gun);
            Destroy(other.gameObject);
            return;
        }
            
    }

    private void SwitchWeapon(WeaponType newWeapon)
    {
        if (newWeapon != currentWeapon)
        {
            currentWeapon = newWeapon;
            ResetAnimations();
            
            return;
        }
    }

}

