using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class ClickToMove : MonoBehaviour
{
    public int selectMouseButton = 0; // Chuột trái
    public int moveMouseButton = 1;   // Chuột phải
    public LayerMask groundMask;
    public LayerMask selectableMask; // Layer chứa các nhân vật có thể chọn

    private Camera cam;
    private bool isSelected = false;

    private TriggerGraphSwitcher2D moveZone;
    private BomAIPositions bomAI;
    private HarmerPositions harmerPositions;
    private GunnerPositions gunnerPositions;

    [Header("Selection Indicator")]
    public GameObject selectionIndicator;

    private void Awake()
    {
        cam = Camera.main;
        if (cam == null)
            Debug.LogError("Không tìm thấy Main Camera!");

        moveZone = GetComponent<TriggerGraphSwitcher2D>();
        bomAI = GetComponent<BomAIPositions>();
        harmerPositions = GetComponent<HarmerPositions>();
        gunnerPositions = GetComponent<GunnerPositions>();

        if (selectionIndicator != null)
            selectionIndicator.SetActive(false);
    }

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(selectMouseButton))
            HandleSelect();

        if (Input.GetMouseButtonDown(moveMouseButton))
            HandleMoveCommand();
    }

    void HandleSelect()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 rayOrigin = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.zero, 0f, selectableMask);

        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            SetSelected(true);
            Debug.Log($"Chọn: {hit.collider.name} tại {rayOrigin}");
        }
        else
        {
            SetSelected(false);
            Debug.Log("Không chọn trúng nhân vật.");
        }
    }

    void HandleMoveCommand()
    {
        if (!isSelected) return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, groundMask);

        if (hit.collider != null)
        {
            Vector3 destination = hit.point;
            Debug.Log($"Di chuyển đến: {destination}");

            if (bomAI != null)
                bomAI.IssuePlayerMoveOrder(destination);
            else if (harmerPositions != null)
                harmerPositions.IssuePlayerMoveOrder(destination);
            else if (gunnerPositions != null)
                gunnerPositions.IssuePlayerMoveOrder(destination);
        }
        else
        {
            Debug.Log("Không trúng mặt đất.");
        }
    }

    void SetSelected(bool v)
    {
        isSelected = v;
        Debug.Log($"{gameObject.name} {(v ? "được chọn" : "bỏ chọn")}");

        if (selectionIndicator != null)
            selectionIndicator.SetActive(v);
    }
}
