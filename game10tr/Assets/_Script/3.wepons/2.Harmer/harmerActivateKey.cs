using System.Collections;
using UnityEngine;

public class activateKey : MonoBehaviour
{
    public appearPrefabs appearPrefabs;
    public DisappearAfterAnim finishedAnimation;
    public float speedHarmerAttack=2;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) )
        {
            StartCoroutine(RepeatAppearUntilFinished());
        }
    }

    IEnumerator RepeatAppearUntilFinished()
    {

        while (finishedAnimation.endedAnimation)
        {
            appearPrefabs.Appear(); // Gọi prefab
            yield return new WaitForSeconds(speedHarmerAttack); // Đợi 0.1s rồi kiểm tra lại (hoặc chờ frame)
        }
    }
}
