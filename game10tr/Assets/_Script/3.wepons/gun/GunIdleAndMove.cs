using UnityEngine;
using Pathfinding;
using System.Collections.Generic;

// Dùng để hiển thị các trạng thái idle và di chuyển của Gunner, khi tấn công sẽ ẩn các trạng thái này đi
public class GunnerIdleAndMoveDisplay : MonoBehaviour
{
    private Dictionary<string, GameObject> displayObjects = new Dictionary<string, GameObject>();

    private SpriteRenderer mainSpriteRenderer;
    private Animator animator;
    private AIPath aiPath;
    private AIMoveToSafeAtkCheckRange atkCheck;
    private GunnerPositions gunnerPositions;
    private GunnerAttackDisplay gunnerAttackDisplay;

    public Vector2 lastDir = Vector2.down;
    private BulletCount bulletCount;
    private Vector2 velocity;

    void Start()
    {
        bulletCount = GetComponent<BulletCount>();
        atkCheck = GetComponent<AIMoveToSafeAtkCheckRange>();
        animator = GetComponent<Animator>();
        mainSpriteRenderer = GetComponent<SpriteRenderer>();
        aiPath = GetComponent<AIPath>();
        gunnerPositions = GetComponent<GunnerPositions>();
        gunnerAttackDisplay = GetComponent<GunnerAttackDisplay>();

        string[] names = { "hand_Up", "hand_Down", "hand_Side", "idle_Up", "idle_Down", "idle_Side" };
        foreach (string name in names)
        {
            Transform t = transform.Find(name);
            if (t != null)
                displayObjects[name] = t.gameObject;
            else
                Debug.LogWarning($"Không tìm thấy object con tên: {name}");
        }
    }

    void Update()
    {
        velocity = aiPath.velocity;

        if (velocity.magnitude > 0.05f)
            lastDir = velocity.normalized;

        string dir = GetDirectionName(lastDir);

        // Nếu đang tấn công → tắt idle/move
        if (gunnerAttackDisplay != null && gunnerAttackDisplay.isAttacking)
        {
            DisableAll();
            DisableAnimator();
            return;
        }

        // Nếu có lệnh từ người chơi
        if (gunnerPositions.playerHasOrder)
        {
            Debug.Log("Gunner has order from player");
            ShowMoveOrIdle(dir);
        }
        else
        {
            Debug.Log("Gunner has no order from player, check attack range");

            if (atkCheck.EnermyInAttackRange&&!bulletCount.outOfBullet)
            {
                // Đang chuẩn bị tấn công → ẩn di chuyển
                DisableAll();
                DisableAnimator();
            }
            else if(atkCheck.closestEnemy==null|| bulletCount.outOfBullet)
            {
                // Không có kẻ địch trong vùng tấn công → idle hoặc di chuyển
                ShowMoveOrIdle(dir);
            }
        }
    }

    void ShowMoveOrIdle(string dir)
    {
        DisableAll();

        if (velocity.magnitude < 0.05f)
        {
            // Đứng yên
            DisableAnimator();
            EnableObject("idle_" + dir);
        }
        else
        {
            // Di chuyển
            EnableAnimator();
            animator.SetFloat("moveX", velocity.x);
            animator.SetFloat("moveY", velocity.y);

            float flipX = (lastDir.x < -0.1f) ? 1 : (lastDir.x > 0.1f) ? -1 : transform.localScale.x;
            transform.localScale = new Vector3(flipX, 1, 1);

            EnableObject("hand_" + dir);
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

    void DisableAll()
    {
        foreach (var obj in displayObjects.Values)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }

    void EnableObject(string name)
    {
        if (displayObjects.TryGetValue(name, out GameObject obj) && obj != null)
        {
            obj.SetActive(true);
        }
    }

    void DisableAnimator()
    {
        if (animator.enabled || mainSpriteRenderer.enabled)
        {
            animator.enabled = false;
            mainSpriteRenderer.enabled = false;
        }
    }

    void EnableAnimator()
    {
        if (!animator.enabled || !mainSpriteRenderer.enabled)
        {
            animator.enabled = true;
            mainSpriteRenderer.enabled = true;
        }
    }
}
