using Pathfinding;
using UnityEngine;

public class PlayerPatrolAndCommand : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRadius = 4f;
    public float patrolDelay = 2f;

    [Header("Detect & Chase Settings")]
    public float detectRadius = 6f;
    public float keepDistance = 2f;

    [Header("Movement Settings")]
    public float speed = 200f;
    public float nextWaypointDistance = 0.5f;

    [Header("Resume Timing")]
    public float resumeDelay = 3f;

    private Vector3 patrolCenter;
    private Vector3 patrolTarget;
    private Transform enemyTarget;
    private Vector3 manualTarget;
    private bool isSelected = false;

    private enum State { Patrol, Chase, Manual }
    private State currentState = State.Patrol;

    private Seeker seeker;
    private Rigidbody2D rb;
    private Path path;
    private int currentWaypoint = 0;

    private float lastPatrolTime = 0f;
    private float lastResumeTime = 0f;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        patrolCenter = transform.position;
        PickNewPatrolPoint();

        InvokeRepeating(nameof(ScanForEnemy), 0f, 0.5f);
        InvokeRepeating(nameof(UpdatePath), 0f, 0.3f);
    }

    void ScanForEnemy()
    {
        if (currentState == State.Manual) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectRadius);
        enemyTarget = null;
        foreach (var h in hits)
        {
            if (h.CompareTag("Enemy"))
            {
                enemyTarget = h.transform;
                currentState = State.Chase;
                return;
            }
        }

        if (currentState == State.Chase)
        {
            lastResumeTime = Time.time;
        }
    }

    void UpdatePath()
    {
        if (!seeker.IsDone()) return;

        switch (currentState)
        {
            case State.Patrol:
                if (Vector2.Distance(transform.position, patrolTarget) < nextWaypointDistance ||
                    Time.time - lastPatrolTime >= patrolDelay)
                {
                    PickNewPatrolPoint();
                    seeker.StartPath(rb.position, patrolTarget, OnPathComplete);
                    lastPatrolTime = Time.time;
                }
                break;

            case State.Chase:
                if (enemyTarget != null)
                {
                    float dist = Vector2.Distance(rb.position, enemyTarget.position);
                    if (dist > keepDistance)
                    {
                        Vector2 dir = (enemyTarget.position - transform.position).normalized;
                        Vector2 stopPoint = (Vector2)enemyTarget.position - dir * keepDistance;
                        seeker.StartPath(rb.position, stopPoint, OnPathComplete);
                    }
                }
                else if (Time.time - lastResumeTime >= resumeDelay)
                {
                    currentState = State.Patrol;
                    lastPatrolTime = Time.time;
                }
                break;

            case State.Manual:
                if (Vector2.Distance(transform.position, manualTarget) < nextWaypointDistance)
                {
                    if (Time.time - lastResumeTime >= resumeDelay)
                    {
                        ExitManual();
                    }
                }
                else
                {
                    seeker.StartPath(rb.position, manualTarget, OnPathComplete);
                }
                break;
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count) return;

        Vector2 dir = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = dir * speed * Time.fixedDeltaTime;
        rb.AddForce(force);

        if (Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
            currentWaypoint++;
    }

    void PickNewPatrolPoint()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 rand = Random.insideUnitCircle * patrolRadius;
            Vector3 candidate = patrolCenter + (Vector3)rand;

            var node = AstarPath.active.GetNearest(candidate).node;
            if (node != null && node.Walkable)
            {
                patrolTarget = candidate;
                return;
            }
        }

        patrolTarget = patrolCenter;
    }

    void OnMouseDown()
    {
        isSelected = !isSelected;
        spriteRenderer.color = isSelected ? Color.cyan : originalColor;

        if (isSelected)
        {
            currentState = State.Manual;
            lastResumeTime = Time.time;
        }
        else
        {
            ExitManual();
        }
    }

    void Update()
    {
        if (isSelected && Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            manualTarget = new Vector3(wp.x, wp.y, 0f);
            currentState = State.Manual;
            patrolCenter = manualTarget;
            PickNewPatrolPoint(); // <-- Cập nhật ngay patrol point mới
            lastResumeTime = Time.time;
        }
    }

    void ExitManual()
    {
        isSelected = false;
        spriteRenderer.color = originalColor;
        currentState = State.Patrol;
        lastPatrolTime = Time.time;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(patrolCenter, patrolRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, keepDistance);
    }
}
