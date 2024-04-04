using UnityEngine;

public class Tile : IHeapItem<Tile>
{
    public bool walkable;
    public Vector2Int gridPosition;
    public Vector3 worldPosition;

    public Tile parentTile;

    public int gCost;
    public int hCost;
    public int fCost { get => hCost + gCost; }

    public int heapIndex;

    public int HeapIndex { get => heapIndex; set => heapIndex = value; }
    public int HCost { get => hCost; set => hCost = value; }

    public Tile(bool walkable, Vector3 worldPosition, int x, int y)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        gridPosition = new Vector2Int(x, y);
    }

    public int CompareTo(Tile other)
    {
        int compare = fCost.CompareTo(other.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(other.hCost);
        }

        return -compare;
    }
}
