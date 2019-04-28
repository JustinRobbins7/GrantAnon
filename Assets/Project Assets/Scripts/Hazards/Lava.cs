using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Lava script, is added to a tile with a Collider2D to damage anything implementing IDamageable nearby
 */
public class Lava : MonoBehaviour
{
    [SerializeField] float lavaDamage;

    /*
    [SerializeField] float damageInterval;
    float damageTimer;
    */

    /**
     * While object is colliding with the attached collider, damages the other object by an amount equal to lavaDamage.
     */
    void OnTriggerStay2D(Collider2D other)
    {
        IDamageable obj = other.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
        if (obj != null)
        {
            obj.OnDamageTaken(lavaDamage);
        }
    }
}
