using UnityEngine;

public enum TileType { Empty, Obstacle, SupplyPoint, Flag }

public class Tile : MonoBehaviour
{
    public Vector2Int position; // x, y
    public TileType tileType;
    public Sprite tileSprite; // sprite gắn trên ô
}
