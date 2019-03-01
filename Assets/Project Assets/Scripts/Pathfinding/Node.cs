using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public string type;
    public Vector3 worldPosition;

    public Node(Vector3 worldPosition, string type) {
        walkable = type != "Obstacles";
        this.type = type;
        this.worldPosition = worldPosition;
    }
}
