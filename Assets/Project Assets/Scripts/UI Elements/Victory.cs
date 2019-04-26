using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * The controller of the Victory screen.
 */
public class Victory : MonoBehaviour
{
    public Text VictoryText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /**
     * Return the players to the menu if they are done with the victory screen
     */
    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            if (Input.GetButtonDown("PAll_Cir"))
            {
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
        }
    }
    
    /**
     * Set which player has won
     */
    public void setText(int player)
    {
        VictoryText.text = "Congratulations Player " + player + "!";
    }
}
