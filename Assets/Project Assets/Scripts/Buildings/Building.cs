using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IDamageable, IBuyable
{
    [SerializeField] float maxHealth;
    [SerializeField] GameObject healthBar = null;
    [SerializeField] int buildCost;
    float currentHealth;

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
            Destroy(gameObject);
        }
    }

    public int GetCost()
    {
        return buildCost;
    }
}
