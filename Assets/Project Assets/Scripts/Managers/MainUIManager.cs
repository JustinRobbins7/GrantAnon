using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{

    /**
     * Proto-manager for the main game UI, display the amount of money a player currently has.
     */
    [SerializeField] Player OwningPlayer = null;

    public Text MoneyDisplay = null;

    void FixedUpdate()
    {
        if (OwningPlayer != null)
        {
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (MoneyDisplay != null)
        {
            MoneyDisplay.text = OwningPlayer.money.ToString();
        }
    }
}
