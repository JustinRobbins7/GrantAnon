using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeBuilding : Building
{
    [SerializeField] float IncomeInterval;
    [SerializeField] int IncomePerInterval;

    [HideInInspector] public Player owner = null;

    float IncomeTimer;

    // Start is called before the first frame update
    /**
     * Sets initial interval times and income per interval if nothing is set in editor
     */
    void Start()
    {
        if (IncomeInterval == 0)
        {
            IncomeInterval = 5.0f;
        }

        if (IncomePerInterval == 0)
        {
            IncomePerInterval = 5;
        }

        IncomeTimer = 0;
    }

    // Update is called once per frame
    /**
    *  Update method called every frame, counts both a timer whose length is determined by 
    *  IncomeInterval. When it reaches zeero, the building's owning player is 
    *  given an amount of money equal to the IncomePerInterval.
    */
    void FixedUpdate()
    {
        IncomeTimer -= Time.deltaTime;

        if (IncomeTimer <= 0)
        {
            MainGameManager.instance.AddPlayerIncome(GetOwningPlayerNum(), IncomePerInterval);
            IncomeTimer = IncomeInterval;
        }
    }

    /**
     *  OnDeath method that overrides the original Building OnDeath method.
     *  Destroys the building and removes itself from its owner's income building list.
     */
    public override void OnDeath()
    {
        if (owner != null)
        {
            owner.incomeBuildings.Remove(gameObject);
        }

        base.OnDeath();
    }
}
