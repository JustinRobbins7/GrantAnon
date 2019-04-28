using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * The Instruction Button takes the player from the main menu to the instruction menu.
 */
public class InstructionButton : MenuButton
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
     * Loads instruction menu when clicked
     */
    override public void onClick()
    {
        //Replace Sample Scene with the instruction Menu scene
        SceneManager.LoadScene("InstructionMenu", LoadSceneMode.Single);
    }
}
