using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Animator animator;
    public float explosionRadius = 2f;
    public int damage = 50;
    public LayerMask enemyLayer;
    public float flySpeed = 10f;

    private Vector3 targetPosition;
    private bool isFlying = false;
    private bool hasExploded = false;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void ThrowTo(Vector3 target)
    {
        targetPosition = target;
        isFlying = true;
        hasExploded = false;
        animator.SetBool("isExplosing", false);
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (isFlying)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, flySpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isFlying = false;
                Explode();
            }
        }
    }

    public void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        animator.SetBool("isExplosing", true);
        DamageEnemies();
    }

    void DamageEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            hit.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void OnExplosionEnd()
    {
        animator.SetBool("isExplosing", false);
        isFlying = false;
        hasExploded = false;
        PoolBom.Instance.ReturnBomb(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}