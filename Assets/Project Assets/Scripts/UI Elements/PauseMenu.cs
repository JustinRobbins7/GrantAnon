using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    bool visibility;
    // Start is called before the first frame update
    void Start()
    {
        visibility = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggle()
    {
        //Debug.LogError("We toggle");
        menu.SetActive(!(menu.activeSelf));
    }
}
