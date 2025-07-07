using UnityEngine;

public class GameController : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.PlayBGM("Gamplay");
    }

}
