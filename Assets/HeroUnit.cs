using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnit : Unit
{
    [HideInInspector] public Player owner = null;
    [SerializeField] float respawnTime;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnDamageTaken(float damageTaken)
    {
        currentHealth -= damageTaken;

        if (healthBar != null)
        {
            healthBar.transform.localScale = new Vector3(currentHealth / maxHealth, 1f, 1f);
        }

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public override void OnDeath()
    {
        selected = false;
        owner.SetHeroRespawn(respawnTime);
        Destroy(gameObject);
    }
}
