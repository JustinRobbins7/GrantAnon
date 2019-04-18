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
    public Text GrantDisplay = null;
    public Text PlayerName = null;

    private void Start()
    {
        PlayerName.text = "Player " + (OwningPlayer.PlayerNumber+1);
    }

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
         if (GrantDisplay != null)
        {
            GrantDisplay.text = OwningPlayer.grant.ToString() + " / 3";
        }
        

    }
}
