using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public string type;
    public int gridCol;
    public int gridRow;

    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    public Node(string type, int gridCol, int gridRow) {
        this.type = type;
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

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare) {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
