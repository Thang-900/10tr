using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float bulletDamage = 20;
    public string enemyTag = "Enemy"; // Tag của kẻ địch để xác định đối tượng nhận sát thương
    private Vector2 direction;
    private GetString classBuff;
    private bool initialized = false;
    

    public void Initialize(Vector2 targetPosition)
    {
        direction = (targetPosition - (Vector2)transform.position).normalized;
    }

    void Update()
    {
        if (!initialized)
        {
            classBuff = FindObjectOfType<GetString>();
            if (classBuff != null)
            {
                if (enemyTag=="Enemy")
                {
                    bulletDamage += classBuff.playerDameBonusRate;
                    Debug.Log($"Player đã tính bulletDamage = {bulletDamage} một lần duy nhất cho Player.");

                }
                else if (enemyTag == "Player")
                {
                    bulletDamage += classBuff.enemyDameBonusRate;
                    Debug.Log($"Enemy đã tính bulletDamage = {bulletDamage} một lần duy nhất cho Enemy.");
                }
                initialized = true;
                Debug.Log($"{gameObject.name} đã tính damageHammer = {bulletDamage} một lần duy nhất.");
            }
            return;
        }
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("da va cham");
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("dan cua " + gameObject.name + " Đã trừ máu kẻ địch " + other.name);
            HealthSystem enemyHealth = other.GetComponentInChildren<HealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(bulletDamage);

                // Sau khi trừ máu, enemy có thể bị Destroy
                if (enemyHealth == null || enemyHealth.gameObject == null)
                {
                    Debug.Log("Enemy đã bị hủy sau khi trừ máu");
                }

                // Trả lại viên đạn về pool
                PoolBomBullet.Instance.ReturnBullet(gameObject);
            }
            else
            {
                Debug.Log("Không tìm thấy HealthSystem trên đối tượng: " + other.name);
            }
        }
        if(other.CompareTag("Block"))
        {
            Debug.Log("dan cua " + gameObject.name + " Đã va chạm với Block " + other.name);
            // Trả lại viên đạn về pool khi va chạm với Block
            PoolBomBullet.Instance.ReturnBullet(gameObject);
        }
    }



    void OnBecameInvisible()
    {
        PoolBomBullet.Instance.ReturnBullet(gameObject); // Nếu ra khỏi màn hình
    }
}
