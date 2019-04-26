using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * IBuyable interface, implemented by scripts that need to have a cost for purchase.
 */
public interface IBuyable
{
    /**
     * Forces implementors to return a integer cost that can be used to purchase the object.
     */
    int GetCost();
}
