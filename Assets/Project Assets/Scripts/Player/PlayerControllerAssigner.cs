using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerAssigner : MonoBehaviour
{
    public int MaxPlayers = 4;

    public Text[] SignInTexts;
    private bool[] ActivePlayers;
    private bool[] ReadyPlayers;

    // Start is called before the first frame update
    void Start()
    {
        ActivePlayers = new bool[SignInTexts.Length];
        ReadyPlayers = new bool[SignInTexts.Length];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < SignInTexts.Length; i++)
        {
            CheckPlayerInput(i + 1);
        }
    }

    void CheckPlayerInput(int ControllerNum)
    {
        int index = ControllerNum - 1;

        if (Input.GetButtonDown("P" + ControllerNum.ToString() + "_X"))
        {
            if (ActivePlayers[index])
            {
                ActivePlayers[index] = true;
                SignInTexts[index].text = "Press X to ready up!";
            }
            else
            {
                ReadyPlayers[index] = true;
                SignInTexts[index].text = "Player " + ControllerNum.ToString() + " Ready!";
            }
        }

        if (Input.GetButtonDown("P" + ControllerNum.ToString() + "_Cir"))
        {
            if (ActivePlayers[index])
            {
                if (ReadyPlayers[index])
                {
                    ReadyPlayers[index] = false;
                    SignInTexts[index].text = "Press X to ready up!";
                }
                else
                {
                    ActivePlayers[index] = false;
                    SignInTexts[index].text = "Press A to join the game!";
                }
            }
        }
    }
}
