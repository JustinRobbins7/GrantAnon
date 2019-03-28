using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeBuilding : Building
{
    [SerializeField] float IncomeInterval;
    [SerializeField] int IncomePerInterval;

     public int OwningPlayerNum;

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
     * Counts down the interval timer, adding funds to the owning player when the timer hits 0.
     */
    void FixedUpdate()
    {
        IncomeTimer -= Time.deltaTime;

        if (IncomeTimer <= 0)
        {
            MainGameManager.instance.AddPlayerIncome(OwningPlayerNum, IncomePerInterval);
            IncomeTimer = IncomeInterval;
        }
    }

    
}
