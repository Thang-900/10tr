using System.Collections;
using UnityEngine;

public class activateKey : MonoBehaviour
{
    public appearPrefabs appearPrefabs;
    public DisappearAfterAnim finishedAnimation;
    public float speedHarmerAttack=2;
    

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            appearPrefabs.Appear();
        }
    }
}
