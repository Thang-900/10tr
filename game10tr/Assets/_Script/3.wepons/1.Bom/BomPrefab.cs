using System;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Animator animator;
    public int damage = 50;
    public LayerMask enemyLayer;
    public float flySpeed = 10f;
    public float arcHeight = 2f;
    public bool canDamage = false;
    public float damageZone = 5f;
    public string enemyTag = "Enemy"; // Tag của kẻ địch để xác định đối tượng nhận sát thương
    public float BomDamage=80; // Sát thương của bom, sẽ được tính toán dựa trên hệ thống đã chọn

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float totalDistance;
    private float flightProgress = 0f;
    private bool isFlying = false;
    private bool hasExploded = false;


    private GetString classBuff;
    private bool initialized = false;


    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        

        canDamage = false;
    }
    
    public void ThrowTo(Vector3 target)
    {

        startPosition = transform.position;
        targetPosition = target;
        totalDistance = Vector3.Distance(startPosition, targetPosition);
        flightProgress = 0f;
        isFlying = true;
        hasExploded = false;
        animator.SetBool("isExplosing", false);
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!initialized)
        {
            classBuff = FindObjectOfType<GetString>();
            if (classBuff != null)
            {
                if(enemyTag == "Player")
                {
                    BomDamage += classBuff.enemyDameBonusRate;
                }
                else if (enemyTag == "Enemy")
                {
                    BomDamage += classBuff.playerDameBonusRate;
                }
                initialized = true;
                Debug.Log($"{gameObject.name} đã tính damageHammer = {BomDamage} một lần duy nhất.");
            }
            return;
        }
        if (isFlying)
        {
            flightProgress += flySpeed * Time.deltaTime / totalDistance;
            flightProgress = Mathf.Clamp01(flightProgress);

            Vector3 nextPos = GetParabolaPosition(startPosition, targetPosition, arcHeight, flightProgress);
            transform.position = nextPos;

            if (flightProgress >= 1f)
            {
                isFlying = false;
                Explode();
            }
        }
        if(canDamage)
        {
            canDamage = false; // Đặt lại canDamage để tránh trừ máu liên tục
            Debug.Log(gameObject.name+ " co the gay sat thuong " );
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, damageZone);
            foreach (var enemy in enemies)
            {
                if (enemy.CompareTag(enemyTag)) // chỉ trừ máu kẻ địch
                {
                    Debug.Log(gameObject.name+ " Đã trừ máu kẻ địch " + enemy.name);
                    HealthSystem enemyHealth = enemy.GetComponentInChildren<HealthSystem>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(BomDamage); // trừ máu theo chỉ số sát thương
                        Debug.Log(gameObject.name+ " da tru " + BomDamage);
                    }
                    else
                    {
                        Debug.Log("Bom Không tìm thấy HealthSystem trên đối tượng: " + enemy.name);
                    }
                }
            }
        }
    }

    Vector3 GetParabolaPosition(Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2f - 1f;
        Vector3 travelDir = end - start;
        Vector3 result = Vector3.Lerp(start, end, t);
        result.y += (-parabolicT * parabolicT + 1f) * height;
        return result;
    }

    public void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        animator.SetBool("isExplosing", true);
        //DamageEnemies();
    }

    //void DamageEnemies()
    //{
    //    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
    //    foreach (Collider2D hit in hits)
    //    {
    //        hit.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
    //    }
    //}

    public void OnExplosionEnd()
    {
        animator.SetBool("isExplosing", false);
        isFlying = false;
        hasExploded = false;
        PoolBomBullet.Instance.ReturnBomb(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageZone);
    }
}
