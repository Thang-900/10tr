using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public Text scoreText;

    void Update()
    {
        if (GameSession.Instance != null)
        {
            float s = GameSession.Instance.GetScore();
            scoreText.text = "Time Score: " + s.ToString("F2");
        }
    }
}
