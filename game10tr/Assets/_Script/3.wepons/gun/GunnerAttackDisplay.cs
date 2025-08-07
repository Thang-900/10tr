using UnityEngine;
using System.Collections.Generic;

public class GunnerAttackDisplay : MonoBehaviour
{
    private Dictionary<string, GameObject> attackObjects = new Dictionary<string, GameObject>();
    private GunnerIdleAndMoveDisplay moveDisplay;
    private BulletCount bulletCount;
    private AIMoveToSafeAtkCheckRange atkCheck;
    private Vector2 directionToEnemy;
    private bool hadEnemyInRangeLastFrame = false;
    private GunnerPositions gunnerPositions;

    public bool changeDirection = false;
    public bool isAttacking = false; // Cờ đang tấn công

    public System.Action OnAttackEnd; // Event báo animation đánh xong

    void Start()
    {
        moveDisplay = GetComponent<GunnerIdleAndMoveDisplay>();
        bulletCount = GetComponent<BulletCount>();
        atkCheck = GetComponent<AIMoveToSafeAtkCheckRange>();
        gunnerPositions = GetComponent<GunnerPositions>();

        string[] names = { "atk_Up", "atk_Down", "atk_Side" };
        foreach (string name in names)
        {
            Transform t = transform.Find(name);
            if (t != null)
                attackObjects[name] = t.gameObject;
            else
                Debug.LogWarning($"Không tìm thấy object tấn công tên: {name}");
        }
        setDirectionOnEven();
    }

    void Update()
    {
        // Nếu đang có lệnh di chuyển từ người chơi, tắt attack và bỏ qua logic attack
        if (gunnerPositions.playerHasOrder)
        {
            DisableAllAttackObjects();
            return;
        }

        bool enemyNow = atkCheck.EnermyInAttackRange;
        bool hasBullet = !bulletCount.outOfBullet;

        // Nếu đang tấn công thì không đổi hướng
        if (isAttacking)
        {
            return;
        }

        // Nếu có enemy và còn đạn → bắt đầu tấn công
        if (enemyNow && hasBullet && atkCheck.closestEnemy != null)
        {
            directionToEnemy = (atkCheck.closestEnemy.position - transform.position).normalized;
            ShowAttackDirection(directionToEnemy);
            StartAttack();
        }
        else
        {
            // Không có enemy hoặc hết đạn → tắt object attack
            DisableAllAttackObjects();
        }

        hadEnemyInRangeLastFrame = enemyNow;
    }

    void ShowAttackDirection(Vector2 dir)
    {
        DisableAllAttackObjects();
        transform.localScale = new Vector3(1, 1, 1);
        string atkDir = GetDirectionName(dir);
        EnableAttackObject("atk_" + atkDir);

        if (atkDir == "Side")
        {
            Vector3 scale = transform.localScale;
            scale.x = dir.x >= 0 ? -1 : 1;
            transform.localScale = scale;
        }
    }

    string GetDirectionName(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return "Side";
        else if (dir.y > 0)
            return "Up";
        else
            return "Down";
    }

    void DisableAllAttackObjects()
    {
        foreach (var obj in attackObjects.Values)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    void EnableAttackObject(string name)
    {
        if (attackObjects.TryGetValue(name, out GameObject obj) && obj != null)
        {
            obj.SetActive(true);
        }
    }

    public void setDirectionOnEven()
    {
        if (!changeDirection)
        {
            changeDirection = true;
        }
    }

    // Bắt đầu animation tấn công
    void StartAttack()
    {
        if (isAttacking) return;
        isAttacking = true;
        // Gọi animation Attack, hoặc enable vfx
        // Tại cuối animation sẽ gọi OnAttackAnimationEnd() qua event
    }

    // Hàm này được gọi ở cuối animation (Animation Event)
    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
        OnAttackEnd?.Invoke(); // Báo cho GunnerPositions
    }
}
