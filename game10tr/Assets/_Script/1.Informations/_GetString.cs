using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetString : MonoBehaviour
{
    // Start is called before the first frame update
    public float playerDameBonusRate;
    public float playerHealthBonusRate;
    public float playerSpeedBonusRate;
    public float enemyDameBonusRate;
    public float enemyHealthBonusRate;
    public float enemySpeedBonusRate;
    void Start()
    {
        string className = ClassSelection.SelectedClassName;
        if (className == "Cach_Mang")
        {
            Debug.Log("Game play selected Class: " + className);
            playerDameBonusRate = 6f;
            playerHealthBonusRate = 0f;
            playerSpeedBonusRate = 0f;
            enemyDameBonusRate = -3f;
            enemyHealthBonusRate = 0f;
            enemySpeedBonusRate = 1f;
        }
        else if (className == "Cong_Dong")
        {
            Debug.Log("Game play selected Class: " + className);
            playerDameBonusRate = 3f;
            playerHealthBonusRate = 11f;
            playerSpeedBonusRate = 0.5f;
            enemyDameBonusRate = 0f;
            enemyHealthBonusRate = 0f;
            enemySpeedBonusRate = 0f;
        }
        else if (className == "Lao_Dong")
        {
            Debug.Log("Game play selected Class: " + className);
            playerDameBonusRate = 0f;
            playerHealthBonusRate = 21f;
            playerSpeedBonusRate = 1f;
            enemyDameBonusRate = 0f;
            enemyHealthBonusRate = 0f;
            enemySpeedBonusRate = 0f;
        }
        else if (className == "Phi_Chu_Nghia")
        {
            Debug.Log("Game play selected Class: " + className);
            playerDameBonusRate = -6f;
            playerHealthBonusRate = -11f;
            playerSpeedBonusRate = -0.5f;
            enemyDameBonusRate = 0f;
            enemyHealthBonusRate = 0;
            enemySpeedBonusRate = 0f;
        }
        else
        {
            Debug.Log("No class selected or class not recognized.");
            playerDameBonusRate = 0f;
            playerHealthBonusRate = 0f;
            playerSpeedBonusRate = 0f;
            enemyDameBonusRate = 0f;
            enemyHealthBonusRate = 0f;
            enemySpeedBonusRate = 0f;
        }
    }
}
