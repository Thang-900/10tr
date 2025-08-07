using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageEven : MonoBehaviour
{
    HarmerDamageZone harmerDamageZone;
    // Start is called before the first frame update
    void Start()
    {
        harmerDamageZone = transform.parent.GetComponentInChildren<HarmerDamageZone>();
    }

    // Update is called once per frame
    public void canGiveDame()
    {
        if (harmerDamageZone!=null)
        {
            harmerDamageZone.canDamage=true;
            Debug.Log("da tru mau ke dich");
        }
        else
        {
            Debug.Log("khong tim thay harmerDamageZone ");
        }

        
    }
}
