using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerAssigner : MonoBehaviour
{
    public int MaxPlayers = 4;
    public GameObject ReadyScreen = null;
    public TestPlayerController PlayerPrefab = null;

    public Text[] SignInTexts;
    private bool[] ActivePlayers;
    private bool[] ReadyPlayers;
    bool AllPlayersReady = false;

    // Start is called before the first frame update
    void Start()
    {
        ActivePlayers = new bool[SignInTexts.Length];
        ReadyPlayers = new bool[SignInTexts.Length];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ReadyScreen != null && ReadyScreen.active)
        {
            AllPlayersReady = true;
            for (int i = 0; i < SignInTexts.Length; i++)
            {
                CheckPlayerInput(i + 1);
                if (!ReadyPlayers[i] && ActivePlayers[i])
                {
                    AllPlayersReady = false;
                }

                if (AllPlayersReady)
                {
                    if (Input.GetButtonDown("PAll_Start"))
                    {
                        StartLevel();
                    }
                    //ReadyScreen.active = false;
                }
            }
        }
    }

    void CheckPlayerInput(int ControllerNum)
    {
        int index = ControllerNum - 1;

        if (Input.GetButtonDown("P" + ControllerNum.ToString() + "_X"))
        {
            Debug.Log("P" + ControllerNum.ToString() + "_X Pressed!");
            if (ActivePlayers[index])
            {
                Debug.Log("Player " + ControllerNum + " Ready!");
                ReadyPlayers[index] = true;
                SignInTexts[index].text = "Player " + ControllerNum.ToString() + " Ready!";
            }
            else 
            {
                Debug.Log("Player " + ControllerNum + " Joined!");
                ActivePlayers[index] = true;
                SignInTexts[index].text = "Press X to ready up!";
            }
        }

        if (Input.GetButtonDown("P" + ControllerNum.ToString() + "_Cir"))
        {
            Debug.Log("P" + ControllerNum.ToString() + "_Cir Pressed!");
            if (ActivePlayers[index])
            {
                if (ReadyPlayers[index])
                {
                    Debug.Log("Player " + ControllerNum + " Unreadied!");
                    ReadyPlayers[index] = false;
                    SignInTexts[index].text = "Press X to ready up!";
                }
                else
                {
                    Debug.Log("Player " + ControllerNum + " Unjoined!");
                    ActivePlayers[index] = false;
                    SignInTexts[index].text = "Press A to join the game!";
                }
            }
        }
    }

    void StartLevel()
    {
        for (int i = 0; i < ReadyPlayers.Length; i++)
        {
            if (ReadyPlayers[i])
            {
                //Spawn Player
            }
        }

        gameObject.active = false;
    }
}
