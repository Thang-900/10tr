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
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        vector2.x = Input.GetAxisRaw("Horizontal");
        vector2.y = Input.GetAxisRaw("Vertical");
        if (vector2.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;  // sang trái
            
            Debug.Log("Moving Left");
        }
        else if (vector2.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false; // sang phải

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
    }
    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.MovePosition(rb.position + vector2 * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
