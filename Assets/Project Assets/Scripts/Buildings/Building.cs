using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IDamageable
{
    [SerializeField] float maxHealth;
    [SerializeField] GameObject healthBar = null;
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
}
