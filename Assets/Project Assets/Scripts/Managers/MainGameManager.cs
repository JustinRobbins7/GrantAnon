using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : GameManager
{
    [SerializeField] float grantInterval;
    [SerializeField] GameObject grantPrefab;
    [SerializeField] Vector2 grantSpawnLocation;

    public static MainGameManager instance = null;

    PlayerController[] Players = null;
    int[] PlayerScores;
    float grantTimer;
    bool runLevel;
    bool spawnedGrant;

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

    public override void StartLevelAfterLoad(int startPlayerCount)
    {

    }
}
