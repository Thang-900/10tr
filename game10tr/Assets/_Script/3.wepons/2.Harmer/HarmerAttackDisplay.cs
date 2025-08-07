using UnityEngine;
using System;
using System.Collections.Generic;

public class HarmerAttackDisplay : MonoBehaviour
{
    private Dictionary<string, GameObject> attackObjects = new Dictionary<string, GameObject>();
    private AIMoveToSafeAtkCheckRange atkCheck;
    private HarmerPositions harmerPositions;
    private Vector2 directionToEnemy;
    private GameObject currentAttackObj;

    public Action OnAttackEnd; // Callback báo khi animation đánh xong
    public bool isAttacking { get; private set; } = false; // Trạng thái tấn công
    void Start()
    {
        atkCheck = GetComponent<AIMoveToSafeAtkCheckRange>();
        harmerPositions = GetComponent<HarmerPositions>();

        // Gán các object tấn công (Up, Down, Side)
        string[] names = { "atk_Up", "atk_Down", "atk_Side" };
        foreach (string name in names)
        {
            Transform t = transform.Find(name);
            if (t != null)
                attackObjects[name] = t.gameObject;
            else
                Debug.LogWarning($"Không tìm thấy object tấn công tên: {name}");
        }

        DisableAllAttackObjects();
    }

    // ====== Gọi từ HarmerPositions để bắt đầu đánh ======
    public void TriggerAttack()
    {
        if (atkCheck.closestEnemy == null || isAttacking) return; // Không đánh nếu đang đánh

        isAttacking = true; // Bắt đầu đánh

        // Tính hướng tấn công
        directionToEnemy = (atkCheck.closestEnemy.position - transform.position).normalized;
        string atkDir = GetDirectionName(directionToEnemy);

        // Bật đúng object và phát animation
        DisableAllAttackObjects();
        if (attackObjects.TryGetValue("atk_" + atkDir, out GameObject atkObj))
        {
            currentAttackObj = atkObj;
            currentAttackObj.SetActive(true);

            // Reset scale Side
            transform.localScale = Vector3.one;
            if (atkDir == "Side")
            {
                Vector3 scale = transform.localScale;
                scale.x = directionToEnemy.x >= 0 ? -1 : 1;
                transform.localScale = scale;
            }

            Animator atkAnimator = currentAttackObj.GetComponent<Animator>();
            if (atkAnimator != null)
            {
                atkAnimator.Play("Attack", -1, 0f); // Chạy lại từ đầu
            }
        }
        else
        {
            currentAttackObj = null;
        }
    }

    // ====== Gọi từ Animation Event ở cuối clip Attack ======
    public void OnAttackAnimationEnd()
    {
        isAttacking = false; // Đánh xong

        // Tắt object sau khi đánh
        if (currentAttackObj != null)
        {
            currentAttackObj.SetActive(false);
            currentAttackObj = null;
        }

        // Báo ngược về HarmerPositions rằng đánh xong
        OnAttackEnd?.Invoke();
    }

    // ====== Helpers ======
    private void DisableAllAttackObjects()
    {
        foreach (var obj in attackObjects.Values)
        {
            if (obj != null) obj.SetActive(false);
        }
        currentAttackObj = null;
    }

    private string GetDirectionName(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return "Side";
        else if (dir.y > 0)
            return "Up";
        else
            return "Down";
    }
}
