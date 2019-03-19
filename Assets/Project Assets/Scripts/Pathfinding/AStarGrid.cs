using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarGrid : MonoBehaviour
{
    Node[,] grid;
    string[] tilePrecedence = { "Obstacles", "LavaHazards", "Grass" };

    // The origin and size of the grid
    Vector2Int origin = new Vector2Int(0, 0);
    Vector2Int size = new Vector2Int(0, 0);

    public void Start() {
        CreateGrid();
    }

    public void CreateGrid() {
        GameObject levelGrid = GameObject.Find("LevelGrid");
        Tilemap[] tilemaps = levelGrid.GetComponentsInChildren<Tilemap>();

        // Iterate through each tilemap to find the origin and size of the grid
        foreach (var tilemap in tilemaps) {
            // If origin of tilemap less than current origin, set origin of tilemap to current origin
            if (origin.x > tilemap.origin.x) {
                origin.x = tilemap.origin.x;
            }
            if (origin.y > tilemap.origin.y) {
                origin.y = tilemap.origin.y;
            }

            // If size of tilemap greater than current size, set size of tilemap to current size
            if (size.x < tilemap.size.x) {
                size.x = tilemap.size.x;
            }
            if (size.y < tilemap.size.y - 1) {
                size.y = tilemap.size.y - 1;
            }
        }

        // Set the grid to the correct size
        grid = new Node[size.x, size.y];

        for (int col = 0; col < size.x; col++) {
            for (int row = 0; row < size.y; row++) {
                foreach (var tilemap in tilemaps) {
                    // Iterate through each row, column, and tilemap type to assign the correct tile type to node and the correct position
                    if (tilemap.HasTile(new Vector3Int(col + origin.x, row + origin.y, 0))) {
                        if (grid[col, row] == null) {
                            grid[col, row] = new Node(tilemap.CellToWorld(new Vector3Int(col + origin.x, row + origin.y, 0)), tilemap.name);
                        } else {
                            /* Ensure that the highest priority tile is set in the list */
                            int gridPrecIndex = Array.FindIndex(tilePrecedence, tileName => tileName.Equals(grid[col, row].type, StringComparison.Ordinal));
                            int tilemapPrecIndex = Array.FindIndex(tilePrecedence, tileName => tileName.Equals(tilemap.name, StringComparison.Ordinal));

                            if (gridPrecIndex > tilemapPrecIndex) {
                                grid[col, row].type = tilemap.name;
                            }
                        }
                    }
                }
            }
        }
    }

    public Node NodeFromWorldPoint(Vector2 worldPosition) {
        int x = Mathf.RoundToInt(worldPosition.x - origin.x);
        int y = Mathf.RoundToInt(worldPosition.y - origin.y);

        if (x < 0) {
            x = 0;
        } else if (x > size.x -1) {
            x = size.x - 1;
        }

        if (y < 0) {
            y = 0;
        } else if (y > size.y - 1) {
            y = size.y - 1;
        }

        return grid[x, y];
    }

    public bool GridInitialized() {
        return grid != null;
    }
}
