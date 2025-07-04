using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance;

    public float score = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Hủy bản mới nếu đã có sẵn
        }
    }

    void Update()
    {
        score += Time.deltaTime;
    }

    public float GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0f;
    }
}
