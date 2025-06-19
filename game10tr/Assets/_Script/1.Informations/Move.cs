using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SmoothGridMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Tilemap[] tilemaps;
    [SerializeField] private float moveSpeed = 5f;
    private Boolean isMoving = true;
    private Rigidbody2D rb;
    private Vector2 vector2;
    public transitions transitions; // Biến để truy cập vào script transitions
    float directionX;
    float directionY;
    public Animator animator;   
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        vector2.x = Input.GetAxisRaw("Horizontal");
        vector2.y = Input.GetAxisRaw("Vertical");
        if (vector2.sqrMagnitude > 1)  // nếu tổng lớn hơn 1 (đi chéo)
            vector2 = vector2.normalized;

        if (vector2.x < 0)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);  // sang trái

            Debug.Log("Moving Left");
        }
        else if (vector2.x > 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);  // sang trái

            Debug.Log("Moving Right");
        }
        //Vector3Int cell = tilemaps[0].WorldToCell(rb.position);
        //foreach (var tm in tilemaps)
        //{
        //    TileBase tile = tm.GetTile(cell);
        //    if (tile != null)
        //    {
        //        Debug.Log($"Tile at {cell} is of type {tile.name} trong {tm.name}");

        //        foreach (var walked in tilemaps)
        //        {
        //            if (tile == walked)
        //            {
        //                Debug.Log($"Tile {tile.name} is in the same tilemap {walked.name}");
        //            }
        //        }
        //    }
        //}
        if (!transitions.isWeaponing)
        {
            if (vector2.x != 0 || vector2.y != 0)
            {
                Debug.Log("dang khong cam vu khi va di chuyen");
                directionX = vector2.x;
                directionY = vector2.y;

                animator.SetBool("isHoldingBomAndWalking", false);
                animator.SetBool("isHoldingBomAndIdling", false);
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdling", false);
            }
            else
            {
                Debug.Log("dang khong cam vu khi va dung yen");
                animator.SetBool("isHoldingBomAndWalking", false);
                animator.SetBool("isHoldingBomAndIdling", false);
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", true);
            }
            
        }
        animator.SetFloat("horizontal", directionX);
        animator.SetFloat("vertical", directionY);

    }
    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.MovePosition(rb.position + vector2 * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
