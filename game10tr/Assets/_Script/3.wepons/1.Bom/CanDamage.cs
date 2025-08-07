using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanDamage : MonoBehaviour
{
    private Bomb bomb;
    // Start is called before the first frame update
    void Start()
    {
        bomb=GetComponent<Bomb>();
    }

    public void BomGiveDame()
    {
        if (bomb != null)
        {
            bomb.canDamage = true;
            Debug.Log("Bom co the tru mau ke dich");
        }
        else
        {
            Debug.Log("khong tim thay harmerDamageZone ");
        }


    }
}
