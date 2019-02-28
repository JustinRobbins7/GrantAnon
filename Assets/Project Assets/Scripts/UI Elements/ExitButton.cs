using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitButton : MenuButton
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void onClick()
    {
        Application.Quit();
    }
}
