using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
  * Class that manages the player sign in menu, assigns player numbers, spawns them, and begins the level.
  */
public class PlayerControllerAssigner : MonoBehaviour
{
    
    public int MaxPlayers = 4;
    public GameObject ReadyScreen;

    public Player aiPlayer;
    public Player player;

    public Vector3[] PlayerSpawns;
    public Text[] SignInTexts;
    public GameObject[] CentralBuildings;
    public GameObject[] IncomeBuildings;
    public GameObject[] HeroUnits;
    public GameObject[] MeleeUnits;

    private bool[] ActivePlayers;
    private bool[] ReadyPlayers;
    bool AllPlayersReady;
    bool SpawningPlayers;
    bool anyPlayersActive;

    [SerializeField] bool SpawnAI;

    AStarGrid grid;
    Vector2Int[] spawnTiles = {
        new Vector2Int(2, -3),
        new Vector2Int(-3, 3),
        new Vector2Int(-3, -3),
        new Vector2Int(3, 3),};

    private int RealPlayers;
    private int AiPlayers;

    void Awake() {
        grid = FindObjectOfType<AStarGrid>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawningPlayers = false;
        AllPlayersReady = false;
        ActivePlayers = new bool[SignInTexts.Length];
        ReadyPlayers = new bool[SignInTexts.Length];
        anyPlayersActive = false;
    }

    /**
     * Inital UI has players join the game, then ready up. 
     * If the start button is pressed when all joined players 
     * are ready, the assigner calls start level.
     */
    // Update is called once per frame
    void FixedUpdate()
    {
        if (ReadyScreen != null && ReadyScreen.active)
        {
            anyPlayersActive = false;
            AllPlayersReady = true;
            for (int i = 0; i < SignInTexts.Length; i++)
            {
                CheckPlayerInput(i + 1);
                if (!ReadyPlayers[i] && ActivePlayers[i])
                {
                    AllPlayersReady = false;
                }

                if (ReadyPlayers[i] || ActivePlayers[i])
                {
                    anyPlayersActive = true;
                }
            }

            if (AllPlayersReady && !SpawningPlayers && anyPlayersActive)
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

    /**
     * Checks each controllers' axes to detect if they are joining the game, readying up, or signing out.
     */
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

    /**
     * Counts the readied up players, then spawns that many players, 
     * initializing them as it does so with their respective controller ID.
     * When done, calls the GameManager's StartLevel method to begin the game then 
     * deactivates itself.
     */
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

        if (MainGameManager.instance != null)
        {
            MainGameManager.instance.InitPlayerArray(PlayersToSpawn);
        }

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
                SpawnedPlayer.GetComponent<Player>().CentralBuildingPrefab = CentralBuildings[i];
                SpawnedPlayer.GetComponent<Player>().IncomeBuildingPrefab = IncomeBuildings[i];
                SpawnedPlayer.GetComponent<Player>().HeroUnitPrefab = HeroUnits[i];
                SpawnedPlayer.GetComponent<Player>().MeleeUnitPrefab = MeleeUnits[i];
                SpawnedPlayer.gameObject.transform.position = PlayerSpawns[i];
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

            
            SpawnedPlayer.transform.position = PlayerSpawns[i];
            SpawnedPlayer.GetComponent<Player>().SetBaseLocation(PlayerSpawns[i]);
            
            //SpawnedPlayer.GetComponent<Player>().SetBaseLocation(ConvertSpawnTileToCoords(spawnTiles[i]));

            MainGameManager.instance.InsertPlayer(i, SpawnedPlayer.GetComponent<Player>());

            MainGameManager.instance.AddPlayerControllerPair(i, ControllerNums[i]);
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

    Vector2 ConvertSpawnTileToCoords(Vector2Int tile) {
        if (tile.x < 0) {
            tile.x = grid.GridSizeVector.x + tile.x;
        }
        if (tile.y < 0) {
            tile.y = grid.GridSizeVector.y + tile.y;
        }

        return grid.Vector2FromGridPosition(tile);
    }
}
