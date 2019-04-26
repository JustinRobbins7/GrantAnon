using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Unit controller, keeps track of all of the units and controls which units attack each other
 */
public class UnitController : MonoBehaviour {
    // Store the location of the gameobjects 
    List<GameObject>[] damageableLocations;
    AStarGrid aStarGrid;
    Queue<GameObject> damageableAddQueue = new Queue<GameObject>();
    HashSet<int> toUpdate = new HashSet<int>();
    int gridSize = 0;
    int gridColSize = 0;

    /**
     * Start method that intializes the UnitController variables
     */
    public void Start() {
        aStarGrid = FindObjectOfType<AStarGrid>();
    }

    /**
     * If a unit has moved, it will be added to the damageableAddQueue. This method adds the unit
     * to the correct index in damageableLocations. At locations where units were added or deleted,
     * this method will iterate through those locations and their neighbors and update the attack
     * targets for any of the units in these positions
     */
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
                    List<GameObject> locations = damageableLocations[index];
                    foreach (GameObject current in locations) {
                        if (current != null && current.GetComponent<Unit>() != null) {
                            FindTarget(current, index);
                        }
                    }
                }
            }
        }
    }

    /**
     * Add a damageable GameObject to the damageableAddQueue
     */
    public void AddDamageable(GameObject damageable) {
        damageableAddQueue.Enqueue(damageable);
    }

    /**
     * Remove a damageable GameObject from its position on the map and update the attack targets
     * for all units at that location and its neighbors (in the next FixedUpdate())
     */
    public bool RemoveDamageable(GameObject damageable, int locationKey) {
        AddUpdateLocations(locationKey);
        return damageableLocations[locationKey].Remove(damageable);
    }

    /**
     * Remove a damageable GameObject from its position on the map and update the attack targets
     * for all units at that location and its neighbors (in the next FixedUpdate())
     */
    public bool RemoveDamageable(GameObject damageable) {
        int locationKey = NodeToKey(aStarGrid.NodeFromWorldPoint(damageable.transform.position));
        return RemoveDamageable(damageable, locationKey);
    }

    /**
     * If a unit has moved, updates its position and update all of its new and old neighbors attack targets
     */
    public void UpdateDamageable(GameObject damageable, Node oldLocation) {
        // If the gameObject exists at the old location then remove it from that location
        int locationKey = NodeToKey(oldLocation);
        RemoveDamageable(damageable, locationKey);
        AddDamageable(damageable);
    }

    /**
     * If the unit is in the first column of the 2D grid return true
     */
    private bool InFirstColumn(int locationKey) {
        return locationKey % gridColSize == 0;
    }

    /**
     * If the unit is in the last column of the 2D grid return true
     */
    private bool InLastColumn(int locationKey) {
        return locationKey % gridColSize == gridColSize - 1;
    }

    /**
     * If the unit is in the first row of the 2D grid return true
     */
    private bool InTopRow(int locationKey) {
        return locationKey < gridColSize;
    }

    /**
     * If the unit is in the last row of the 2D grid return true
     */
    private bool InBottomRow(int locationKey) {
        return locationKey + gridColSize > gridSize;
    }

    /**
     * Finds an attackable target for a unit within its current tile or any of its neighbor tiles
     */
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

    /**
     * Returns a list of integers corresponding to the indexes in the damageableLocations array that are neighbors of
     * or are the passed in locationKey
     */
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

    /**
     * For each neighbor of a given location, this method adds that to the list of tiles to update in the next fixedUpdate()
     */
    private void AddUpdateLocations(int locationKey) {
        List<int> neighbors = GetNeighborsAndSelf(locationKey);

        foreach (int neighbor in neighbors) {
            toUpdate.Add(neighbor);
        }
    }

    /**
     * Converts a node location to a key representing the index of the node in a one dimensional array
     */
    private int NodeToKey(Node node) {
        return node.gridRow * gridColSize + node.gridCol;
    }
}
