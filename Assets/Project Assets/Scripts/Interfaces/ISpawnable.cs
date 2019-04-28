using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Interface implemented to show that the implmentor has an owning player that spawned it.
 */
public interface ISpawnable
{
    /**
    * Sets the implementing script's owning player number.
    */
    void SetOwningPlayerNum(int owningPlayerNum);

    /**
    * Returns an integer associated with the player number of the owner of the implementor.
    */
    int GetOwningPlayerNum();
}
