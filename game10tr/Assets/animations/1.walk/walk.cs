using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walk : MonoBehaviour
{
    public Animator walking;

    // Update is called once per frame
    public void Walk()
    {

        float horizontal = Input.GetAxisRaw("Horizontal"); // -1 (trái), 1 (phải)
        float vertical = Input.GetAxisRaw("Vertical");     // -1 (xuống), 1 (lên)

        walking.SetBool("isWDown", vertical < 0);
        walking.SetBool("isWUp", vertical > 0);
        walking.SetBool("isWSide", Mathf.Abs(horizontal) != 0);

    }
}
