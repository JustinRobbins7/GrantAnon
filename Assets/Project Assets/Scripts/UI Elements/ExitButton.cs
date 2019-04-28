using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Exit Button is a kind of Menu Button that will exit the game when clicked.
 */
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

    /**
     * Quits application when called
     */
    override public void onClick()
    {
        Application.Quit();
    }
}
