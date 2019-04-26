using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A node of the grid
 */
public class Node : IHeapItem<Node>
{
    public string type;
    public int gridCol;
    public int gridRow;

    public int gCost;
    public int hCost;
    public Node parent;
    int heapIndex;

    /**
     * Constructor for the Node
     */
    public Node(string type, int gridCol, int gridRow) {
        this.type = type;
        this.gridCol = gridCol;
        this.gridRow = gridRow;
    }

    /**
     * Whether or not the node is walkable (or whether it has to be navigated around
     */
    public bool walkable {
        get {
            return !type.Equals("Obstacles", StringComparison.OrdinalIgnoreCase);
        }
    }

    /**
     * fCost is a value used for A* pathfinding
     */
    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    /**
     * The index in the heap
     */
    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    /**
     * How to compare two nodes
     */
    public int CompareTo(Node nodeToCompare) {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

    /**
     * Whether two nodes represent the same location in the grid
     */
    public bool IsEqual(Node node) {
        return (node.gridRow == gridRow && node.gridCol == gridCol);
    }
}
