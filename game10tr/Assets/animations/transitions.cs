using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transitions : MonoBehaviour
{
    public Animator animator;
    private enum Direction { Up, Down, Side }
    private Direction lastDirection = Direction.Side;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        bool isMoving = (Mathf.Abs(horizontal) > 0f || Mathf.Abs(vertical) > 0f);

        if (isMoving)
        {
            // Xác định hướng cuối cùng
            if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            {
                lastDirection = Direction.Side;
            }
            else
            {
                if (vertical > 0) lastDirection = Direction.Up;
                else if (vertical < 0) lastDirection = Direction.Down;
            }

            // Xử lý Walk
            animator.SetBool("isWUp", lastDirection == Direction.Up);
            animator.SetBool("isWDown", lastDirection == Direction.Down);
            animator.SetBool("isWSide", lastDirection == Direction.Side);

            // Tắt Idle
            animator.SetBool("isIUp", false);
            animator.SetBool("isIDown", false);
            animator.SetBool("isISide", false);
        }
        else
        {
            // Không di chuyển, dùng hướng cuối cùng để idle đúng
            animator.SetBool("isIUp", lastDirection == Direction.Up);
            animator.SetBool("isIDown", lastDirection == Direction.Down);
            animator.SetBool("isISide", lastDirection == Direction.Side);

            // Tắt Walk
            animator.SetBool("isWUp", false);
            animator.SetBool("isWDown", false);
            animator.SetBool("isWSide", false);
        }
    }

}
