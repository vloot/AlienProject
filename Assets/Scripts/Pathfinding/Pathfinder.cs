using UnityEngine;
using System.Collections.Generic;
using System;

public class Pathfinder
{
    private LevelGrid levelGrid;

    private Vector2Int[] _offsets;

    public Pathfinder(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
        _offsets = new Vector2Int[] {
            TileNeighbors.Top, TileNeighbors.Right, TileNeighbors.Bottom, TileNeighbors.Left,
            TileNeighbors.TopRight, TileNeighbors.BottomRight, TileNeighbors.BottomLeft, TileNeighbors.TopLeft
        };
    }

    public List<Tile> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        return FindPath(levelGrid.GetTile(startPos), levelGrid.GetTile(targetPos));
    }

    public List<Tile> FindPath(Tile startTile, Tile targetTile)
    {
        var open = new Heap<Tile>(levelGrid.TilesCount);
        var closed = new HashSet<Tile>();

        open.Add(startTile);

        while (open.Count > 0)
        {
            var currentTile = open.RemoveFirst();
            closed.Add(currentTile);

            if (currentTile == targetTile)
            {
                // done
                return ReconstructPath(startTile, targetTile);
            }

            foreach (var n in GetNeighbours(currentTile))
            {
                if (closed.Contains(n))
                {
                    continue;
                }

                int newCost = currentTile.gCost + GetDistance(currentTile, n);
                if (newCost < n.gCost || !open.Contains(n))
                {
                    n.gCost = newCost;
                    n.hCost = GetDistance(n, targetTile);
                    n.parentTile = currentTile;

                    if (!open.Contains(n))
                    {
                        open.Add(n);
                    }
                }
            }
        }

        // no path found
        return new List<Tile>();
    }

    private int GetDistance(Tile currentTile, Tile targetTile)
    {
        var distX = Mathf.Abs(currentTile.gridPosition.x - targetTile.gridPosition.x);
        var distY = Mathf.Abs(currentTile.gridPosition.y - targetTile.gridPosition.y);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }

        return 14 * distX + 10 * (distY - distX);
    }

    public List<Tile> ReconstructPath(Tile startTile, Tile targetTile)
    {
        var path = new List<Tile>();
        var currentTile = targetTile;

        while (currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parentTile;
        }

        path.Reverse();
        return path;
    }

    public List<Tile> GetNeighbours(Tile tile)
    {
        var neighbours = new List<Tile>();
        AddNeighboursToList(neighbours, tile);
        return neighbours;
    }

    private void AddNeighboursToList(List<Tile> neighbours, Tile tile)
    {
        for (int offsetIndex = 0; offsetIndex < _offsets.Length; offsetIndex++)
        {
            var position = tile.gridPosition + _offsets[offsetIndex];

            var neighbourTile = levelGrid.GetTile(position);
            if (tile.walkable) // check if tile exists?
            {
                neighbours.Add(neighbourTile);
            }
        }
    }
}