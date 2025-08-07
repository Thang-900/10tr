using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmerAtkEven: MonoBehaviour
{
    private HarmerAttackDisplay harmerAttackDisplay;
    private void Start()
    {
        harmerAttackDisplay = GetComponentInParent<HarmerAttackDisplay>();
    }
    public void OnAttackAnimationEnd()
    {
        harmerAttackDisplay.OnAttackAnimationEnd();
    }
}
