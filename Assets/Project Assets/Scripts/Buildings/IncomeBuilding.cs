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
