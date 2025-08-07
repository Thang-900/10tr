using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerAttackDirectionFinder : MonoBehaviour
{
    private GunnerAttackDisplay gunnerAttackDisplay;
    private void Start()
    {
        gunnerAttackDisplay=GetComponentInParent<GunnerAttackDisplay>();
    }
    public void ChangeDirectionOnEven()
    {
        gunnerAttackDisplay.OnAttackAnimationEnd();
    }
}
