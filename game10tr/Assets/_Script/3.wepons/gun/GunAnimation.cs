using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_Animation : MonoBehaviour
{
    public transitions transitions;
    public Animator animator;

    public bool isHoldingGun = false;

    float vertical;
    float horizontal;
    float directionX;
    float directionY;

    public GameObject sideScriptGun;
    public GameObject upScriptGun;
    public GameObject downScriptGun;

    public GameObject sidePrefabGun;
    public GameObject upPrefabGun;
    public GameObject downPrefabGun;

    

    private void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");

        if (isHoldingGun && transitions.isWeaponing)
        {
            //SpriteRenderer sr = GetComponent<SpriteRenderer>();
            //if (sr != null) sr.enabled = true;

            if (vertical != 0 || horizontal != 0)
            {
                Debug.Log("Đang cầm súng và di chuyển");

                directionX = horizontal;
                directionY = vertical;
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
            if ((directionX == 1 || directionX == -1) && directionY == 0)
            {
                sideScriptGun.SetActive(true);
                upScriptGun.SetActive(false);
                downScriptGun.SetActive(false);
            }
            else if (directionY == 1)
            {
                sideScriptGun.SetActive(false);
                upScriptGun.SetActive(true);
                downScriptGun.SetActive(false);
            }
            else if (directionY == -1)
            {
                sideScriptGun.SetActive(false);
                upScriptGun.SetActive(false);
                downScriptGun.SetActive(true);
            }
            if (animator != null)
            {
                animator.SetFloat("horizontal", directionX);
                animator.SetFloat("vertical", directionY);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gun") && transitions != null && !transitions.isWeaponing)
        {
            Debug.Log("Súng đã được nhặt");
            isHoldingGun = true;
            transitions.isWeaponing = true;
            Destroy(other.gameObject);
        }
    }
}
