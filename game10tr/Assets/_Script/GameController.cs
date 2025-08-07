using UnityEngine;

public class GameController : MonoBehaviour
{
    void Start()
    {
        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM("Primal (Bonus Theme)");
        }
        
    }

}
