using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * The IndicatorButton is used to change the visibility of the button.
 */
public class IndicatorButton : MonoBehaviour
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
     * onSelect() makes the Indicator fully visible
     */
    public void onSelect()
    {
        this.GetComponent<Image>().color = new Color32(255, 255,255, 255);
    }

    /**
     * onUnselect() makes the Indicator grayed out, and not very visible.
     */
    public void onUnselect()
    {
        this.GetComponent<Image>().color = new Color32(183, 183, 183, 120);
    }
    
}
