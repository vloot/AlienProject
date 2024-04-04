using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int gridWorldSize;
    [SerializeField] private float nodeRadius;
    [SerializeField] private LayerMask unwalkableMask;
    [SerializeField] private Transform playerTransform;

    private Tile[,] grid;
    private float nodeDiameter;
    private Vector2Int gridSize;
    private int tilesCount;

    public int TilesCount { get => tilesCount; private set => tilesCount = value; }

    private Pathfinder pathfinder;

    private void Awake()
    {
        pathfinder = new Pathfinder(this);
    }

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSize.x = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSize.y = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Tile[gridSize.x, gridSize.y];
        var worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 -
            Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                var worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                    Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask); // TODO add baking
                grid[x, y] = new Tile(walkable, worldPoint, x, y);
            }
        }

        TilesCount = gridSize.x * gridSize.y;
    }

    public Tile TileFromWorldPoint(Vector3 pos)
    {
        Vector2 percent;
        percent.x = Mathf.Clamp01((pos.x + gridWorldSize.x / 2) / gridWorldSize.x);
        percent.y = Mathf.Clamp01((pos.z + gridWorldSize.y / 2) / gridWorldSize.y);

        Vector2Int tilePos = new Vector2Int(
            Mathf.RoundToInt((gridSize.x - 1) * percent.x),
            Mathf.RoundToInt((gridSize.y - 1) * percent.y)
        );

        return grid[tilePos.x, tilePos.y];
    }

    public Tile GetTile(Vector2Int tileIndex)
    {
        tileIndex.x = Mathf.Clamp(tileIndex.x, 0, gridSize.x - 1);
        tileIndex.y = Mathf.Clamp(tileIndex.y, 0, gridSize.y - 1);
        return grid[tileIndex.x, tileIndex.y];
    }

    public Tile GetTile(Vector3 worldPosition)
    {
        return TileFromWorldPoint(worldPosition);
    }

    public bool HasTile(Vector2Int pos)
    {
        return pos.x < gridSize.x && pos.y < gridSize.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid == null) return;

        var size = Vector3.one * (nodeDiameter - 0.4f);
        var playerTile = TileFromWorldPoint(playerTransform.position);

        var lst = pathfinder.FindPath(start.position, target.position);

        foreach (var tile in lst)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(tile.worldPosition, size);
        }

        foreach (var tile in grid)
        {
            if (lst.Contains(tile)) continue;

            var color = tile.walkable ? Color.cyan : Color.red;
            color = playerTile == tile ? Color.green : color;
            Gizmos.color = color;

            Gizmos.DrawWireCube(tile.worldPosition, size);
        }

    }


    [SerializeField] private Transform start;
    [SerializeField] private Transform target;
    [SerializeField] private bool findPath;
}
