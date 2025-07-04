using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Animator animator;
    public float explosionRadius = 2f;
    public int damage = 50;
    public LayerMask enemyLayer;
    public float flySpeed = 10f;
    public float arcHeight = 2f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float totalDistance;
    private float flightProgress = 0f;
    private bool isFlying = false;
    private bool hasExploded = false;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
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
