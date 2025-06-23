using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_Animation : MonoBehaviour
{
    
    public Animator animator;

    public bool isHoldingGun = false;

    private float vertical;
    private float horizontal;

    public GetInput getInput; // Biến để truy cập vào script GetInput

    public GameObject sideScriptGun;
    public GameObject upScriptGun;
    public GameObject downScriptGun;

    public GameObject sidePrefabGun;
    public GameObject upPrefabGun;
    public GameObject downPrefabGun;

    private Vector2 direction;

    public void Gun_Animation(bool isWeaponing)
    {
        if (isHoldingGun && isWeaponing)
        {
            vertical = Input.GetAxisRaw("Vertical");
            horizontal = Input.GetAxisRaw("Horizontal");
            direction = getInput.GetDirection();
            if (vertical != 0 || horizontal != 0)
            {
                Debug.Log("Đang cầm súng và di chuyển");
                if (animator != null)
                {
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isIdling", false);
                    animator.SetBool("isGunWalking", true);
                }
            }
            else
            {
                if (animator != null)
                {
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isIdling", true);
                    animator.SetBool("isGunWalking", false);
                }
            }
            if ((direction.x == 1 || direction.x == -1) && direction.y == 0)
            {
                sideScriptGun.SetActive(true);
                upScriptGun.SetActive(false);
                downScriptGun.SetActive(false);
            }
            else if (direction.y == 1)
            {
                sideScriptGun.SetActive(false);
                upScriptGun.SetActive(true);
                downScriptGun.SetActive(false);
            }
            else if (direction.y == -1)
            {
                sideScriptGun.SetActive(false);
                upScriptGun.SetActive(false);
                downScriptGun.SetActive(true);
            }
            if (animator != null)
            {
                animator.SetFloat("horizontal", direction.x);
                animator.SetFloat("vertical", direction.y);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gun"))
        {
            Debug.Log("Súng đã được nhặt");
            isHoldingGun = true;
            
            Destroy(other.gameObject);
        }
    }
}
