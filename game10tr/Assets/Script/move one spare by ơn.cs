using UnityEngine;
using UnityEngine.Tilemaps;

public class SmoothGridMovement : MonoBehaviour
{
    [Header("Thiết lập Tilemap")]
    [SerializeField] private Tilemap[] tilemaps;    // Mảng Tilemap để kiểm tra (có thể nhiều lớp)

    [Header("Thông số di chuyển")]
    [SerializeField] private float stepSize = 0.1f;  // Kích thước mỗi bước di chuyển (đơn vị world)
    [SerializeField] private float moveSpeed = 5f;   // Tốc độ mượt khi di chuyển tới target

    private Vector3 targetPos;   // Vị trí đích hiện tại
    private bool isMoving = false;

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
                isMoving = false;
            return;
        }

        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) dir = Vector3.up;
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) dir = Vector3.down;
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) dir = Vector3.left;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) dir = Vector3.right;

        if (dir != Vector3.zero)
        {
            Vector3 nextPos = transform.position + dir * stepSize;
            Vector3Int cell = tilemaps[0].WorldToCell(nextPos);

            bool canWalk = false;
            foreach (var tm in tilemaps)
            {
                TileBase tile = tm.GetTile(cell);
                if (tile != null)
                {
                    Debug.Log($"Tile at {cell} is of type {tile.name} trong {tm.name}");
                    canWalk = true;
                    break;
                }
            }

            if (canWalk)
            {
                targetPos = nextPos;
                isMoving = true;
            }
        }
    }
}
