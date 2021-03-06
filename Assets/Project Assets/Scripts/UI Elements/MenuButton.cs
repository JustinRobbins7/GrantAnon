﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * The Menu Button is the parent class of all menu buttons types.
 * It allows buttons to show when they have been selected and unselected.
 */
public abstract class MenuButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Changes color when selected and deselected
     */
    public void onSelect()
    {
        this.GetComponent<Image>().color = new Color32(215, 180, 0, 180);
    }
    public void onUnselect()
    {
        this.GetComponent<Image>().color = new Color32(138, 25, 19, 180);
    }
    public abstract void onClick();
}
