using UnityEngine;

public class Soldier : MonoBehaviour
{
    // Thông tin cá nhân
    public string soldierName;
    public int maxHP;
    public int currentHP;
    public int moveRange;
    public int attackPower;
    public int morale;

    // Trạng thái
    public bool isAlive = true;
    public bool hasMoved = false;
    public bool hasAttacked = false;

    // Hàm hành động (sẽ hoàn thiện sau)
    public void Move() { }
    public void Attack() { }
    public void Die() { }
}
