﻿using UnityEngine;

public class setATK : MonoBehaviour
{
    public bool isATK = false;

    public void StartAttack()
    {
        isATK = true;
        Invoke("EndAttack", 0.5f); // Thời gian phù hợp với animation đòn đánh
    }

    void EndAttack()
    {
        isATK = false;
        Debug.Log("Kết thúc tấn công, isATK = " + isATK);
    }
}
