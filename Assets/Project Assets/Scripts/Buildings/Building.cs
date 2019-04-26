using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Building parent class, implements IDamageable, IBuyable, and ISpawnable
 */
public class Building : MonoBehaviour, IDamageable, IBuyable, ISpawnable
{
    [SerializeField] float maxHealth;
    [SerializeField] GameObject healthBar = null;
    [SerializeField] int buildCost;
    float currentHealth;
    int owningPlayerNum;

    void Start()
    {
        currentHealth = maxHealth;
    }

    /**
     * Implemented OnDamageTaken method from IDamageable
     */
    public void OnDamageTaken(float damageTaken)
    {
        damageTaken -= currentHealth;

        if (healthBar != null)
        {
            healthBar.transform.localScale = new Vector3(currentHealth / maxHealth, 1f, 1f);
        }

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    /**
     * Implemented GetCost method from IBuyable
     */
    public int GetCost()
    {
        return buildCost;
    }

    /**
     * Implemented ISpawnable method SetOwningPlayerNum
     */
    public void SetOwningPlayerNum(int owningPlayerNum) {
        this.owningPlayerNum = owningPlayerNum;
    }

    /**
     * Implemented ISpawnable method GetOwningPlayerNum
     */
    public int GetOwningPlayerNum() {
        return owningPlayerNum;
    }

    /**
     * Overridable method OnDeath, used when the building's health drops to zero.
     * 
     * By default it destroys the gameobject when health equals zero.
     */
    public virtual void OnDeath()
    {
        FindObjectOfType<UnitController>().RemoveDamageable(gameObject);
        Destroy(gameObject);
    }
}
