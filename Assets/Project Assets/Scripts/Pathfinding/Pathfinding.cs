using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    AStarGrid grid;
    PathRequestManager requestManager;

    void Awake() {
        grid = GetComponent<AStarGrid>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector2 startPos, Vector2 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector2 startPos, Vector2 endPos) {
        Vector2[] waypoints = new Vector2[0];
        bool success = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(endPos);

        if (startNode.walkable && targetNode.walkable) {
            Heap<Node> openSet = new Heap<Node>(grid.GridSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0) {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode) {
                    success = true;
                    break;
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
                        } else {
                            openSet.UpdateItem(neighbor);
                        }
                    }
                }
            }
        }

        yield return null;

        if (success) {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, success);
    }

    Vector2[] RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        
        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector2[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    Vector2[] SimplifyPath(List<Node> path) {
        List<Vector2> waypoints = new List<Vector2>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++) {
            Vector2 directionNew = new Vector2(path[i - 1].gridCol - path[i].gridCol, path[i - 1].gridRow - path[i].gridRow);
            if (directionNew != directionOld) {
                waypoints.Add(grid.NodeToWorldPosition(path[i]));
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
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
