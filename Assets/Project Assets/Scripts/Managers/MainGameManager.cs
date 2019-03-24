using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : GameManager
{
    [SerializeField] protected float grantInterval;
    [SerializeField] protected GameObject grantPrefab;
    [SerializeField] protected Vector2 grantSpawnLocation;

    public static MainGameManager instance = null;

    protected PlayerController[] Players = null;
    protected int[] PlayerScores;
    protected float grantTimer;
    protected bool runLevel;
    protected bool spawnedGrant;
    protected Dictionary<int, int> zeroBasedPlayerToController;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

    public void SpawnGrant()
    {
        Debug.Log("Spawning grant!");
        GameObject grantObject = Instantiate(grantPrefab);
        grantObject.transform.position = grantSpawnLocation;
        grantObject.layer = SortingLayer.GetLayerValueFromName("Foreground");

        spawnedGrant = true;
    }

    public void ScoreGrant(int scoringPlayer)
    {
        int playerIndex = scoringPlayer - 1;
        if (0 >= playerIndex && playerIndex < PlayerScores.Length)
        {
            PlayerScores[playerIndex]++;
        }

        spawnedGrant = false;
    }

    public override void StartLevel(int startPlayerCount)
    {
        PlayerCount = startPlayerCount;
        grantTimer = 0.0f;

        spawnedGrant = false;
        runLevel = true;
    } 

    public void InitPlayerArray(int playerCount)
    {
        Players = new PlayerController[playerCount];
    }

    public void InsertPlayer(int playerIndex, PlayerController newPlayer)
    {
        Players[playerIndex] = newPlayer;
    }

    public void AddPlayerIncome(int earningPlayer, int moneyEarned)
    {
        int playerIndex = earningPlayer - 1;
        if (playerIndex <= 0 && playerIndex < Players.Length)
        {
           //Debug.Log("PlayerIndex: " + playerIndex.ToString());
            Players[playerIndex].money += moneyEarned;
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
