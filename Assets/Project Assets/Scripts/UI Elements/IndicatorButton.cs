using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void onSelect()
    {
        this.GetComponent<Image>().color = new Color32(255, 255,255, 255);
    }
    public void onUnselect()
    {
        this.GetComponent<Image>().color = new Color32(183, 183, 183, 120);
    }
    
}
