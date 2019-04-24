using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {
    // Store the location of the gameobjects 
    List<GameObject>[] damageableLocations;
    AStarGrid aStarGrid;
    Queue<GameObject> damageableAddQueue = new Queue<GameObject>();
    HashSet<int> toUpdate = new HashSet<int>();
    int gridSize = 0;
    int gridColSize = 0;

    public void Start() {
        aStarGrid = FindObjectOfType<AStarGrid>();
    }

    public void FixedUpdate() {
        if (gridSize == 0 && aStarGrid.GridInitialized()) {
            gridSize = aStarGrid.GridSize;
            gridColSize = aStarGrid.GridSizeVector.x;
            damageableLocations = new List<GameObject>[gridSize];
            for (int i = 0; i < damageableLocations.Length; i++) {
                damageableLocations[i] = new List<GameObject>();
            }
        }

        if (gridSize != 0) {
            while (damageableAddQueue.Count > 0) {
                GameObject damageable = damageableAddQueue.Dequeue();
                int locationKey = NodeToKey(aStarGrid.NodeFromWorldPoint(damageable.transform.position));
                damageableLocations[locationKey].Add(damageable);
                AddUpdateLocations(locationKey);
            }

            if (toUpdate.Count > 0) {
                int[] updateArray = new int[toUpdate.Count];
                toUpdate.CopyTo(updateArray);
                toUpdate = new HashSet<int>(); // Reset to ensure the same things aren't unnecessarily updated again

                // Update targets for all units at these locations
                foreach (int index in updateArray) {
                    foreach (GameObject current in damageableLocations[index]) {
                        if (current != null && current.GetComponent<Unit>() != null) {
                            FindTarget(current, index);
                        }
                    }
                }
            }
        }
    }

    public void AddDamageable(GameObject damageable) {
        damageableAddQueue.Enqueue(damageable);
    }

    public bool RemoveDamageable(GameObject damageable, int locationKey) {
        AddUpdateLocations(locationKey);
        return damageableLocations[locationKey].Remove(damageable);
    }

    public bool RemoveDamageable(GameObject damageable) {
        int locationKey = NodeToKey(aStarGrid.NodeFromWorldPoint(damageable.transform.position));
        return RemoveDamageable(damageable, locationKey);
    }

    public void UpdateDamageable(GameObject damageable, Node oldLocation) {
        // If the gameObject exists at the old location then remove it from that location
        int locationKey = NodeToKey(oldLocation);
        RemoveDamageable(damageable, locationKey);
        AddDamageable(damageable);
    }

    private bool InFirstColumn(int locationKey) {
        return locationKey % gridColSize != 0;
    }

    private bool InLastColumn(int locationKey) {
        return locationKey % gridColSize != gridColSize - 1;
    }

    private bool InTopRow(int locationKey) {
        return locationKey < gridColSize;
    }

    private bool InBottomRow(int locationKey) {
        return locationKey + gridColSize > gridSize;
    }

    private void FindTarget(GameObject current, int locationKey) {
        List<GameObject> gos = new List<GameObject>();

        List<int> neighbors = GetNeighborsAndSelf(locationKey);
        int playerNumber = current.GetComponent<Unit>().GetOwningPlayerNum();

        foreach (int neighbor in neighbors) {
            foreach (GameObject target in damageableLocations[neighbor]) {
                if (target != null && !target.Equals(current)) {
                    int targetPlayerNumber = -1;
                    if (target.GetComponent<Unit>() != null) {
                        targetPlayerNumber = target.GetComponent<Unit>().GetOwningPlayerNum();
                    } else if (target.GetComponent<Building>() != null) {
                        targetPlayerNumber = target.GetComponent<Building>().GetOwningPlayerNum();
                    }
                    if (targetPlayerNumber != -1 && playerNumber != targetPlayerNumber) {
                        current.GetComponent<Unit>().SetAttackTarget(target);
                        return;
                    }
                }
            }
        }
    }

    private List<int> GetNeighborsAndSelf(int locationKey) {
        List<int> neighbors = new List<int>();

        // Top left
        if (!InFirstColumn(locationKey) && !InTopRow(locationKey)) {
            neighbors.Add(locationKey - gridColSize - 1);
        }

        // Top
        if (!InTopRow(locationKey)) {
            neighbors.Add(locationKey - gridColSize);
        }

        // Top right
        if (!InLastColumn(locationKey) && !InTopRow(locationKey)) {
            neighbors.Add(locationKey - gridColSize + 1);
        }

        // Left
        if (!InFirstColumn(locationKey)) {
            neighbors.Add(locationKey - 1);
        }

        // Current
        neighbors.Add(locationKey);

        // Right
        if (!InLastColumn(locationKey)) {
            neighbors.Add(locationKey + 1);
        }

        // Bottom Left
        if (!InFirstColumn(locationKey) && !InBottomRow(locationKey)) {
            neighbors.Add(locationKey + gridColSize - 1);
        }

        // Bottom
        if (!InBottomRow(locationKey)) {
            neighbors.Add(locationKey + gridColSize);
        }

        // Bottom Right
        if (!InLastColumn(locationKey) && !InBottomRow(locationKey)) {
            neighbors.Add(locationKey + gridColSize + 1);
        }

        return neighbors;
    }

    private void AddUpdateLocations(int locationKey) {
        List<int> neighbors = GetNeighborsAndSelf(locationKey);

        foreach (int neighbor in neighbors) {
            toUpdate.Add(neighbor);
        }
    }

    private int NodeToKey(Node node) {
        return node.gridRow * gridColSize + node.gridCol;
    }
}
