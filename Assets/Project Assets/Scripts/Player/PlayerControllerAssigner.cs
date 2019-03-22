using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerAssigner : MonoBehaviour
{
    public int MaxPlayers = 4;
    public GameObject ReadyScreen;

    public Player aiPlayer;
    public Player player;

    public Vector3[] PlayerSpawns;
    public Text[] SignInTexts;
    private bool[] ActivePlayers;
    private bool[] ReadyPlayers;
    bool AllPlayersReady;
    bool SpawningPlayers;
    [SerializeField] bool SpawnAI;

    private int RealPlayers;
    private int AiPlayers;

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
        int[] ControllerNums = new int[4];

        RealPlayers = 0;

        for (int i = 0; i < ReadyPlayers.Length; i++)
        {
            if (ReadyPlayers[i])
            {
                RealPlayers++;
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

        AiPlayers = SpawnAI ? ControllerNums.Length - RealPlayers : 0;

        Debug.Log("Need to spawn " + RealPlayers.ToString() + " players");

        MainGameManager.instance.InitPlayerArray(PlayersToSpawn);

        // Spawn real players
        for (int i = 0; i < PlayersToSpawn; i++)
        {
            if (i < 0 || i >= PlayerSpawns.Length)
            {
                Debug.Log("ERROR: Players Spawn does not exist for Player " + i.ToString() + "!");
                return;
            }

            Player SpawnedPlayer;

            if (IsPlayer(i, PlayersToSpawn)) { /* Spawn regular player */
                SpawnedPlayer = Instantiate(player);
                SpawnedPlayer.GetComponent<PlayerController>().SetCameraViewport(i + 1, RealPlayers);
                SpawnedPlayer.GetComponent<PlayerController>().SetControllerNumber(ControllerNums[i]);
            } else { /* Spawn AI player */
                SpawnedPlayer = Instantiate(aiPlayer);
            }

            SpawnedPlayer.GetComponent<Player>().PlayerNumber = i;

            GameObject PlayerUnitRoot = new GameObject();
            PlayerUnitRoot.name = "Player " + (i + 1) + " Units";
            SpawnedPlayer.GetComponent<Player>().UnitRoot = PlayerUnitRoot;

            GameObject PlayerBuildingRoot = new GameObject();
            PlayerBuildingRoot.name = "Player " + (i + 1) + " Buildings";
            SpawnedPlayer.GetComponent<Player>().BuildingRoot = PlayerBuildingRoot;

            MainGameManager.instance.InsertPlayer(i, SpawnedPlayer.GetComponent<Player>());
        }

        MainGameManager.instance.StartLevel(PlayersToSpawn);

        gameObject.SetActive(false);
    }

    bool IsPlayer(int index, int playersToSpawn) {
        return index < RealPlayers;
    }

    int PlayersToSpawn {
        get {
            return RealPlayers + AiPlayers;
        }
    }

}
