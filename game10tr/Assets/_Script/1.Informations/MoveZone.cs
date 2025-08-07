using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class TriggerGraphSwitcher2D : MonoBehaviour
{
    private Seeker seeker;
    private string currentArea = ""; // Lưu vùng hiện tại (Ground/HighGround)
    public bool isOnGround = false; // Biến này có thể dùng để kiểm tra trạng thái
    public bool isOnHighGround = false; // Biến này có thể dùng để kiểm tra trạng thái

    void Start()
    {
        seeker = GetComponent<Seeker>();
        if (seeker == null)
        {
            Debug.LogError("Cần gắn component Seeker vào nhân vật!");
        }
    }

    private void SetGraphOnce(string area)
    {
        // Chỉ set nếu khác vùng hiện tại
        if (currentArea == area) return;

        currentArea = area;

        if (area == "Ground")
        {
            if (!isOnGround)
            {
                isOnGround = true;
                isOnHighGround = false; // Reset trạng thái HighGround
            }
            Debug.Log($"{name} → GroundGraph");
            seeker.graphMask = GraphMask.FromGraphName("GroundGraph");
            Debug.Log($"{name} → GroundGraph");
        }
        else if (area == "HighGround")
        {
            if (!isOnHighGround)
            {
                isOnHighGround = true;
                isOnGround = false; // Reset trạng thái Ground
            }
            Debug.Log($"{name} → HighGroundGraph");
            seeker.graphMask = GraphMask.FromGraphName("HighGroundGraph");
            Debug.Log($"{name} → HighGroundGraph");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            SetGraphOnce("Ground");
        }
        else if (other.CompareTag("HighGround"))
        {
            SetGraphOnce("HighGround");
        }
    }
}
