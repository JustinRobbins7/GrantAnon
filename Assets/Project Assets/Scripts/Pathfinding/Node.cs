using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public string type;
    public Vector2 worldPosition;

    public Node(Vector2 worldPosition, string type) {
        walkable = type != "Obstacles";
        this.type = type;
        this.worldPosition = worldPosition;
    }
}
