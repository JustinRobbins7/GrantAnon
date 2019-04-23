using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnit : Unit
{
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
        base.OnDamageTaken(damageTaken);
    }

    public override void OnDeath()
    {
        owner.SetHeroRespawn(respawnTime);
        base.OnDeath();
    }
}
