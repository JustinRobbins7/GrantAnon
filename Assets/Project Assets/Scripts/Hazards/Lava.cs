using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] float lavaDamage;

    /*
    [SerializeField] float damageInterval;
    float damageTimer;
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
