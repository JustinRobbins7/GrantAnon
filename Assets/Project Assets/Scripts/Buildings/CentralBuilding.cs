using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Central Building class, child of Building.
 * 
 * An invincible structure given to each player at the start of the game,
 * the Central Building serves as a spawn point and a reliable, albeit slow
 * source of income for each player
 */
public class CentralBuilding : Building
{
    [SerializeField] float IncomeInterval;
    [SerializeField] int IncomePerInterval;

    float IncomeTimer;

    /**
    *  Start method, initializes values for CentralBuilding
    */
    // Start is called before the first frame update
    void Start()
    {
        if (IncomeInterval == 0)
        {
            IncomeInterval = 10.0f;
        }

        if (IncomePerInterval == 0)
        {
            IncomePerInterval = 5;
        }

        IncomeTimer = IncomeInterval;
    }

    /**
    *  Update method called every frame, counts both a timer whose length is determined by 
    *  IncomeInterval. When it reaches zeero, the building's owning player is 
    *  given an amount of money equal to the IncomePerInterval.
    */
    // Update is called once per frame
    void Update()
    {
        IncomeTimer -= Time.deltaTime;

        if (IncomeTimer <= 0)
        {
            MainGameManager.instance.AddPlayerIncome(GetOwningPlayerNum(), IncomePerInterval);
            IncomeTimer = IncomeInterval;
        }
    }
}
