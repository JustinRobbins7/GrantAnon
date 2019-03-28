using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : GameManager
{
    /**
     * MainGameManager for the project, handles players and the flow of the game.
     */
    [SerializeField] protected float grantInterval;
    [SerializeField] protected GameObject grantPrefab;
    [SerializeField] protected Vector2 grantSpawnLocation;

    public static MainGameManager instance = null;

    protected Player[] Players = null;
    protected int[] PlayerScores;
    protected float grantTimer;
    protected bool runLevel;
    protected bool spawnedGrant;
    protected Dictionary<int, int> zeroBasedPlayerToController;

    /**
     * Ensures the GameManager is unique and accessible from code anywhere
     */

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        runLevel = false;
        zeroBasedPlayerToController = new Dictionary<int, int>();
    }

    // Update is called once per frame
    /**
     * If the level is active, counts down the grant timer, once it reaches zero, it spawns a grant 
     * and waits for it to be collected before beginning a new timer.
     */
    void FixedUpdate()
    {
        if (runLevel)
        {
            if (!spawnedGrant)
            {
                grantTimer += Time.deltaTime;
            }

            if (grantTimer >= grantInterval)
            {
                SpawnGrant();
                grantTimer = 0.0f;
            }
        }
    }

    /**
     * Spawns a grant at a specificed position
     */
    public void SpawnGrant()
    {
        Debug.Log("Spawning grant!");
        GameObject grantObject = Instantiate(grantPrefab);
        grantObject.transform.position = grantSpawnLocation;
        grantObject.layer = SortingLayer.GetLayerValueFromName("Foreground");

        spawnedGrant = true;
    }

    /**
     * Scores a grant for the specified player
     */
    public void ScoreGrant(int scoringPlayer)
    {
        int playerIndex = scoringPlayer;
        Debug.Log(scoringPlayer.ToString());
        if (0 >= playerIndex && playerIndex < PlayerScores.Length)
        {
            PlayerScores[playerIndex]++;
        }

        spawnedGrant = false;
    }

    /**
     * Begins the new level
     */
    public override void StartLevel(int startPlayerCount)
    {
        PlayerCount = startPlayerCount;
        grantTimer = 0.0f;
        PlayerScores = new int[startPlayerCount];

        spawnedGrant = false;
        runLevel = true;
    }

    /**
     * Initializing player array based on the supplied player count
     */
    public void InitPlayerArray(int playerCount)
    {
        Players = new Player[playerCount];
    }


    /**
     * Inserts a new player into the player array
     */
    public void InsertPlayer(int playerIndex, Player newPlayer)
    {
        Debug.Log(playerIndex.ToString());
        Players[playerIndex] = newPlayer;
    }

    /**
     * Adds a specified amount of money to the specified player.
     */
    public void AddPlayerIncome(int earningPlayer, int moneyEarned)
    {
        if (earningPlayer >= 0 && earningPlayer < Players.Length)
        {
           //Debug.Log("PlayerIndex: " + playerIndex.ToString());
            Players[earningPlayer].money += moneyEarned;
        }
    }

    public void ResumeLevel()
    {
        runLevel = true;
    }

    public void AddPlayerControllerPair(int playerNum, int controllerNum)
    {
        zeroBasedPlayerToController.Add(playerNum, controllerNum);
    }

    public int GetPlayerControllerZeroBased(int PlayerNum)
    {
        return zeroBasedPlayerToController[PlayerNum];
    }

    public int GetNumPlayers()
    {
        return Players.Length;
    }
}
