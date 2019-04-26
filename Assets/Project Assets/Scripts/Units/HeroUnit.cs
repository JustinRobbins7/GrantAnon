using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  The hero units class is a child class of the Unit class that has a respawn time 
 *  and calls its owning player to respawn it once it dies.
 */
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

    /**
     * Overridden OnDeath method, performs the same actions as Unit after notifying its owner to begin its respawn timer
     */
    public override void OnDeath()
    {
        owner.SetHeroRespawn(respawnTime);
        base.OnDeath();
    }
}
