using UnityEngine;

public class GameController : MonoBehaviour
{
    private int turnCount = 0;
    private bool isPlayerTurn = true;

    public void NextTurn()
    {
        turnCount++;
        isPlayerTurn = !isPlayerTurn;
    }

    public void CheckGameEnd()
    {
        // Kiểm tra điều kiện thắng/thua
    }

    public void CalculateCasualtyRate()
    {
        // Tính tỉ lệ hy sinh
    }
}
