using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapCheck : MonoBehaviour
{
    [SerializeField] public Tilemap[] tilemaps; // Mảng các Tilemap để kiểm tra
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var tilemap in tilemaps)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
            TileBase tile = tilemap.GetTile(cellPosition);

            if(tile != null)
            {
                Debug.Log($"Tile at {cellPosition} is of type {tile.name} trong {tilemap.name}");
                break;
            }
        }
    }
}
