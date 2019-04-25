using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IDamageable, IBuyable, ISpawnable
{
    [SerializeField] float maxHealth;
    [SerializeField] GameObject healthBar = null;
    [SerializeField] static int buildCost = 10;
    float currentHealth;
    int owningPlayerNum;

    void Start()
    {
        currentHealth = maxHealth;
    }

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

    public static int GetCost()
    {
        return buildCost;
    }

    int IBuyable.GetCost() {
        return Building.GetCost();
    }

    public void SetOwningPlayerNum(int owningPlayerNum) {
        this.owningPlayerNum = owningPlayerNum;
    }

    public int GetOwningPlayerNum() {
        return owningPlayerNum;
    }

    public virtual void OnDeath()
    {
        FindObjectOfType<UnitController>().RemoveDamageable(gameObject);
        Destroy(gameObject);
    }
}
