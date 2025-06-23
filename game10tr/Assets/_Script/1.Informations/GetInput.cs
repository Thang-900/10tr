using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInput : MonoBehaviour
{
    private float vertical;
    private float horizontal;
    private Vector2 vector2;

    public Animator animator; // Biến để truy cập vào Animator

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        
    }
    public Vector3 GetDirection()
    {
        if (vertical != 0 || horizontal != 0)
        {
            Debug.Log($"x = {vertical}, y = {horizontal}");

            vector2.x = horizontal;
            vector2.y = vertical;

        }
        return vector2;
    }
    public void setAnimation(float ver, float hor)
    {
        animator.SetFloat("vertical", ver);
        animator.SetFloat("horizontal", hor);
    }
}
