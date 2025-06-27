using UnityEngine;
using Pathfinding;

public class AIFindTargetInRange : MonoBehaviour
{
    public float searchRadius = 5f;
    public string targetTag = "Target";
    public float checkInterval = 0.5f;
    public float stopDistance = 1f;

    private AIDestinationSetter destinationSetter;
    private   AIPath aiPath;
    private Transform currentTarget = null;
    private bool isControlledByAI = false;
    

    void Start()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();
        InvokeRepeating(nameof(CheckForTarget), 0f, checkInterval);
    }

    void Update()
    {
        // Nếu đang không bị AI chiếm quyền, cho phép click chuột phải để di chuyển
        if (!isControlledByAI && Input.GetMouseButtonDown(1))
        {
            Debug.Log("da huy ai va cho nguoi choi di chuyen");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                destinationSetter.target = null; // xóa target cũ
                aiPath.destination = hit.point;
                aiPath.SearchPath();
            }
        }

        // Nếu AI đang tự di chuyển và đã đến gần target thì trả lại quyền cho người chơi
        if (isControlledByAI && currentTarget != null)
        {
            Debug.Log("dang dung ai va da den target");
            float dist = Vector3.Distance(transform.position, currentTarget.position);
            if (dist <= stopDistance)
            {
                Debug.Log("AI đã đến target, trả lại quyền cho người chơi");
                isControlledByAI = false;
                destinationSetter.target = null;
                currentTarget = null;

                // ✅ Reset đường đi để cho phép click chuột tiếp
                aiPath.destination = aiPath.position;
                aiPath.SearchPath();
            }
        }
    }

    void CheckForTarget()
    {
        // Nếu đã bị AI chiếm quyền, không tìm thêm nữa
        if (isControlledByAI) return;

        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        GameObject closest = null;
        float closestDist = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist <= searchRadius && dist < closestDist)
            {
                closest = target;
                closestDist = dist;
            }
        }

        if (closest != null)
        {
            currentTarget = closest.transform;
            destinationSetter.target = currentTarget;
            isControlledByAI = true; // chiếm quyền điều khiển
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
