using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerAssigner : MonoBehaviour
{
    public int MaxPlayers = 4;
    public GameObject ReadyScreen;

    public PlayerController PlayerPrefab;
    //public GameObject PlayerGroup;

    public Vector3[] PlayerSpawns;
    public Text[] SignInTexts;
    private bool[] ActivePlayers;
    private bool[] ReadyPlayers;
    bool AllPlayersReady;
    bool SpawningPlayers;

    // Start is called before the first frame update
    void Start()
    {
        SpawningPlayers = false;
        AllPlayersReady = false;
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
            }

            if (AllPlayersReady && !SpawningPlayers)
            {
                if (Input.GetButtonDown("PAll_Start"))
                {
                    SpawningPlayers = true;
                    StartLevel();
                }
                //ReadyScreen.active = false;
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
                    SignInTexts[index].text = "Press X to join the game!";
                }
            }
        }
    }

    void StartLevel()
    {
        int PlayersToSpawn = 0;
        int[] ControllerNums = new int[4];

        for (int i = 0; i < ReadyPlayers.Length; i++)
        {
            if (ReadyPlayers[i])
            {
                PlayersToSpawn++;

                for (int j = 0; j < ControllerNums.Length; j++)
                {
                    if(ControllerNums[j] == 0)
                    {
                        ControllerNums[j] = i + 1;
                        break;
                    }
                }
            }
        }
        Debug.Log("Need to spawn " + PlayersToSpawn.ToString() + " players");

        for (int i = 1; i <= PlayersToSpawn; i++)
        {

            if (i - 1 < 0 || i - 1 >= PlayerSpawns.Length)
            {
                Debug.Log("ERROR: Players Spawn does not exist for Player " + i.ToString() + "!");
                return;
            }
            
            PlayerController SpawnedPlayer = null;
            SpawnedPlayer = Instantiate(PlayerPrefab);

            SpawnedPlayer.SetControllerNumber(ControllerNums[i - 1]);
            SpawnedPlayer.SetCameraViewport(i, PlayersToSpawn);

            SpawnedPlayer.gameObject.transform.position = PlayerSpawns[i - 1];

            GameObject PlayerUnitRoot = new GameObject();
            PlayerUnitRoot.name = "Player " + ControllerNums[i - 1].ToString() + " Units";
            SpawnedPlayer.UnitRoot = PlayerUnitRoot;

            /*
            GameObject SpawnedGroup = null;
            SpawnedGroup = Instantiate(PlayerGroup);
            SpawnedGroup.gameObject.transform.position = PlayerSpawns[i - 1];

            PlayerController SpawnedPlayer = null;
            SpawnedPlayer = GetComponentInChildren<PlayerController>(true);

            if (SpawnedPlayer != null)
            {
                SpawnedPlayer.SetControllerNumber(ControllerNums[i - 1]);
                SpawnedPlayer.SetCameraViewport(i, PlayersToSpawn);
            }
            else
            {
                SpawnedPlayer = GetComponent<PlayerController>();
                if (SpawnedPlayer != null)
                {
                    SpawnedPlayer.SetControllerNumber(ControllerNums[i - 1]);
                    SpawnedPlayer.SetCameraViewport(i, PlayersToSpawn);
                }
                else
                {
                    Debug.Log("Could not find Player Controller!");
                }
            }
            */
            
        }

        gameObject.SetActive(false);
    }

}
