using UnityEngine;

public class TileNeighbors
{
    public static Vector2Int TopLeft = new(1, 1);
    public static Vector2Int Top = new(1, 0);
    public static Vector2Int TopRight = new(1, -1);
    public static Vector2Int Left = new(0, 1);
    public static Vector2Int Right = new(0, -1);
    public static Vector2Int BottomLeft = new(-1, 1);
    public static Vector2Int Bottom = new(-1, 0);
    public static Vector2Int BottomRight = new(-1, -1);
}
