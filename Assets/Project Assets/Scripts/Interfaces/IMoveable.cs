using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * IMovable is a interface attached to scripts that needs to have the ability to move around the map.
 * This is primarily used by Units to call pathfinding scripts.
 */
public interface IMoveable
{
    /**
     * Given a Vector2 destination, move there (or perform some other action given a Vector2).
     */
    void Move(Vector2 target);
}
