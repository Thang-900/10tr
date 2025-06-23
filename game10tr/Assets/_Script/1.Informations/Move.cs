using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Move : MonoBehaviour
{
    public void movePlayer(float x,float y,Animator animator,bool isWeaponing)
    {
        Vector2 vector2 = new Vector2(x, y);    
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
        if (!isWeaponing)
        {
            if (vector2.x != 0 || vector2.y != 0)
            {
                Debug.Log("dang khong cam vu khi va di chuyen");

                animator.SetBool("isWalking", true);
                animator.SetBool("isIdling", false);
            }
            else
            {
                Debug.Log("dang khong cam vu khi va dung yen");

                animator.SetBool("isWalking", false);
                animator.SetBool("isIdling", true);
            }
        }     
    }
    
}
