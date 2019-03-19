using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public string type;
    public Vector2 worldPosition;
    public int gridCol;
    public int gridRow;

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(Vector2 worldPosition, string type, int gridCol, int gridRow) {
        this.type = type;
        this.worldPosition = worldPosition;
        this.gridCol = gridCol;
        this.gridRow = gridRow;
    }

    public bool walkable {
        get {
            return !type.Equals("Obstacles", StringComparison.OrdinalIgnoreCase);
        }
    }

    public int fCost {
        get {
            return gCost + hCost;
        }
    }
}
