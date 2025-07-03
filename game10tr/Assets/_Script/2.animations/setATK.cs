using UnityEngine;

public class setATK : MonoBehaviour
{
    public bool isATK = false;

    void OnEnable()
    {
        isATK = true;
        Invoke("EndAttack", 0.5f); // Thời gian phù hợp với animation đòn đánh
    }

    void EndAttack()
    {
        isATK = false;
    }
}
