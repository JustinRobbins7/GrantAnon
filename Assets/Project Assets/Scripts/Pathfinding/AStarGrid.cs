﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * Implementation of the A* pathfinding algorithm
 */
public class AStarGrid : MonoBehaviour {
    Node[,] grid;
    string[] tilePrecedence = { "Obstacles", "LavaHazards", "Grass" };

    // The origin and size of the grid
    Vector2Int origin = new Vector2Int(0, 0);
    Vector2Int size = new Vector2Int(0, 0);
    Vector2 tileExtents = new Vector2(0.732f, 0.47f);

    public void Start() {
        CreateGrid();
    }

    /**
     * Creates a 2D grid from the tilemap
     */
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
                            grid[col, row] = new Node(tilemap.name, col, row);
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

    /**
     * Gets all of the neighbors of a node (diagonal and up,down,left,right)
     */
    public List<Node> GetNeighbors(Node node) {
        List<Node> neighbors = new List<Node>();

        for (int col = -1; col <= 1; col++) {
            for (int row = -1; row <= 1; row++) {
                if (col == 0 && row == 0) {
                    continue;
                }

                int checkCol = node.gridCol + col;
                int checkRow = node.gridRow + row;

                if (checkCol >= 0 && checkCol < size.x && checkRow >= 0 && checkCol < size.y) {
                    neighbors.Add(grid[checkCol, checkRow]);
                }
            }
        }

        return neighbors;
    }

    /**
     * Converts grid position to world position
     */
    public Vector2 Vector2FromGridPosition(Vector2Int gridPos) {
        return NodeToWorldPosition(grid[gridPos.x, gridPos.y]);
    }

    /**
     * Converts tile position to world position
     */
    public Vector2 TileToWorld(Vector2 tilePos) {
        Vector2 worldPos;

        worldPos.x = tileExtents.x * (tilePos.x - tilePos.y) * .5f;
        worldPos.y = tileExtents.y * (tilePos.x + tilePos.y) * .5f;

        return worldPos;
    }

    /**
     * Converts tile position to world position
     */
    public Vector2 WorldToTile(Vector2 worldPos) {
        Vector2 tilePos;

        tilePos.x = (worldPos.x / tileExtents.x + worldPos.y / tileExtents.y);
        tilePos.y = (worldPos.y / tileExtents.y - worldPos.x / tileExtents.x);

        return tilePos;
    }

    /**
     * Converts node position to world position
     */
    public Vector2 NodeToWorldPosition(Node node) {
        int tileX = node.gridCol + origin.x;
        int tileY = node.gridRow + origin.y;

        return TileToWorld(new Vector2(tileX, tileY));
    }

    /**
     * Converts grid position to node
     */
    public Node NodeFromGridPosition(Vector2Int gridPos) {
        return grid[gridPos.x, gridPos.y];
    }

    /**
     * Converts node from tile position
     */
    public Node NodeFromTilePoint(Vector2 tilePos) {
        int x = Mathf.RoundToInt(tilePos.x - origin.x);
        int y = Mathf.RoundToInt(tilePos.y - origin.y);

        if (x < 0) {
            x = 0;
        } else if (x > size.x - 1) {
            x = size.x - 1;
        }

        if (y < 0) {
            y = 0;
        } else if (y > size.y - 1) {
            y = size.y - 1;
        }

        return NodeFromGridPosition(new Vector2Int(x, y));
    }

    /**
     * Converts world point to node
     */
    public Node NodeFromWorldPoint(Vector2 worldPos) {
        Vector2 tilePos = WorldToTile(worldPos);

        return NodeFromTilePoint(tilePos);
    }

    /**
     * Boolean of if the grid has been initialized or not
     */
    public bool GridInitialized() {
        return grid != null;
    }

    /**
     * Returns a vector representing the size of the grid
     */
    public Vector2Int GridSizeVector {
        get {
            return size;
        }
    }

    /**
     * Returns the number of tiles in the grid
     */
    public int GridSize {
        get {
            return size.x * size.y;
        }
    }
}
