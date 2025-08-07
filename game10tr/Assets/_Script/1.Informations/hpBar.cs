using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Pathfinding;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public GameObject diePrefab;

    private AIPath aiPath;

    [Header("Health Bar")]
    public Transform fillBar;
    private float initialScaleX;

    private bool isDead = false;

    void Start()
    {
        aiPath = GetComponent<AIPath>();
        StartCoroutine(InitializeHealth());
    }
    private IEnumerator InitializeHealth()
    {
        // Đợi 1 frame để Start() của GetString chạy xong
        yield return null;

        GetString classBuff = FindObjectOfType<GetString>();
        if (classBuff != null&&gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Player"))
            {
                maxHealth = Mathf.Max(0, Mathf.RoundToInt(maxHealth + classBuff.playerHealthBonusRate));
                if (aiPath != null)
                {
                    aiPath.maxSpeed += classBuff.playerSpeedBonusRate; // Tăng tốc độ di chuyển của Player
                }
                Debug.Log($"Player đã tính maxHealth = {maxHealth} một lần duy nhất với classBuff.");
            }
            else if (gameObject.CompareTag("Enemy"))
            {
                maxHealth = Mathf.Max(0, Mathf.RoundToInt(maxHealth + classBuff.enemyHealthBonusRate));
                if (aiPath != null)
                {
                    aiPath.maxSpeed += classBuff.enemySpeedBonusRate; // Tăng tốc độ di chuyển của Player
                }
                Debug.Log($"Enemy đã tính maxSpeed = {aiPath.maxSpeed} một lần duy nhất với classBuff.");
            }
            currentHealth = maxHealth;
            Debug.Log($"{gameObject.name} khởi tạo với maxHealth = {maxHealth}, currentHealth = {currentHealth}");
        }
        else if(classBuff == null)
        {
            Debug.LogWarning("Không tìm thấy classBuff (GetString) trong scene.");
            currentHealth = maxHealth;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} không phải là Player, giữ nguyên maxHealth = {maxHealth}");
            currentHealth = maxHealth;
        }
        if (fillBar != null)
        {
            initialScaleX = fillBar.localScale.x;
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: fillBar chưa được gán trong Inspector");
        }

        UpdateHealthBar();
    }
   
    public void buff()
    {
        maxHealth += 20f; 
        Heal(20);
    }
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"{gameObject.name} nhận sát thương {damage}. Máu còn lại: {currentHealth}");

        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"{gameObject.name} được hồi máu {amount}. Máu hiện tại: {currentHealth}");
        UpdateHealthBar();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} đã chết.");

        // Reset mục tiêu của các AI khác
        AIMoveToSafeAtkCheckRange[] allAI = FindObjectsOfType<AIMoveToSafeAtkCheckRange>();
        foreach (var ai in allAI)
        {
            if (ai.closestEnemy == transform)
            {
                ai.ResetTarget();
                Debug.Log($"AI {ai.name} đã reset target vì {gameObject.name} chết.");
            }
        }

        if (diePrefab != null)
        {
            Instantiate(diePrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void UpdateHealthBar()
    {
        if (fillBar != null)
        {
            if (maxHealth > 0f)
            {
                float percent = currentHealth / maxHealth;

                if (float.IsNaN(percent))
                {
                    Debug.LogError($"{gameObject.name}: phần trăm máu bị NaN. currentHealth = {currentHealth}, maxHealth = {maxHealth}");
                }

                Vector3 scale = fillBar.localScale;
                scale.x = initialScaleX * percent;
                fillBar.localScale = scale;
            }
            else
            {
                Debug.LogError($"{gameObject.name}: maxHealth <= 0! Không thể cập nhật thanh máu.");
            }
        }
    }
}
