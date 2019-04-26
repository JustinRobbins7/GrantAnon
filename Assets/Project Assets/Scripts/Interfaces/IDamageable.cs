using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * IDamageable interface that is implemented so that the implementing script can respond to damage.
 */
public interface IDamageable
{
    /**
     * OnDamageTaken required that the implementor be able to respond to a float amount of damage taken.
     * 
     * Usually this is by decrementing a health variable found in the implementing script, 
     * then destroying the implementing object when their health is equal to or less than 0.
     */
    void OnDamageTaken(float damageTaken);
}
