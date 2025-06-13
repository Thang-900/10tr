using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class idle : MonoBehaviour
{
    enum IdleState
    {
        up,
        down,
        side
    }
    IdleState Istate = IdleState.side;
    public Animator idling;
    public void Idle()
    {

        float horizontal = Input.GetAxisRaw("Horizontal"); // -1 (trái), 1 (phải)
        float vertical = Input.GetAxisRaw("Vertical");     // -1 (xuống), 1 (lên)
        if (vertical < 0)
        {
            Istate = IdleState.down;
            Debug.Log("Idle Down");
        }
        else if (vertical > 0)
        {

            Istate = IdleState.up;
            Debug.Log("Idle Up");
        }
        else if (Mathf.Abs(horizontal) != 0)
        {
            Istate = IdleState.side;
            Debug.Log("Idle Side");

        }
        switch(Istate)
        {
            case IdleState.up:
                idling.SetBool("isIUp", true);
                idling.SetBool("isIDown", false);
                idling.SetBool("isISide", false);
                break;
            case IdleState.down:
                idling.SetBool("isIUp", false);
                idling.SetBool("isIDown", true);
                idling.SetBool("isISide", false);
                break;
            case IdleState.side:
                idling.SetBool("isIUp", false);
                idling.SetBool("isIDown", false);
                idling.SetBool("isISide", true);
                break;
        }



    }
}
