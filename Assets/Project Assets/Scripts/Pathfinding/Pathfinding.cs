using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    AStarGrid grid;

    void Awake() {
        grid = GetComponent<AStarGrid>();
    }

    public List<Node> FindPath(Vector2 startPos, Vector2 endPos) {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(endPos);

        Heap<Node> openSet = new Heap<Node>(grid.GridSize);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in grid.GetNeighbors(currentNode)) {
                if (!neighbor.walkable || closedSet.Contains(neighbor)) {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor)) {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        
        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }

    int GetDistance(Node nodeA, Node nodeB) {
        int dstCol = Mathf.Abs(nodeA.gridCol - nodeB.gridCol);
        int dstRow = Mathf.Abs(nodeA.gridRow - nodeB.gridRow);

        if (dstCol > dstRow) {
            return 14 * dstRow + 10 * (dstCol - dstRow);
        }
        return 14 * dstCol + 10 * (dstRow - dstCol);
    }
}
