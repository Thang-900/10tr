using Pathfinding;
using UnityEngine;

public class EnemyScannerWithChase : MonoBehaviour
{
    public float searchRadius = 5f;
    public float speed = 200f;
    public float nextWaypointDistance = 0.5f;
    public float returnDelay = 1.5f;

    private Transform target;
    private Vector3 startPosition;
    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private bool returningHome = false;

    private Seeker seeker;
    private Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        InvokeRepeating("ScanForPlayer", 0f, 0.5f); // Tìm player mỗi 0.5s
        InvokeRepeating("UpdatePath", 0f, 0.2f);     // Cập nhật đường đi mỗi 0.2s
    }

    void ScanForPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, searchRadius);
        bool foundPlayer = false;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                target = hit.transform;
                foundPlayer = true;
                returningHome = false;
                break;
            }
        }

        if (!foundPlayer)
        {
            if (!returningHome)
            {
                target = null;
                returningHome = true;
                Invoke("ReturnHome", returnDelay);
            }
        }
    }

    void ReturnHome()
    {
        if (returningHome)
        {
            seeker.StartPath(rb.position, startPosition, OnPathComplete);
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if (target != null)
            {
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }
            else if (returningHome)
            {
                seeker.StartPath(rb.position, startPosition, OnPathComplete);
            }
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
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }

        reachedEndOfPath = false;

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;
        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
