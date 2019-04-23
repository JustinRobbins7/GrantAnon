using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainGameManager : GameManager
{
    /**
     * MainGameManager for the project, handles players and the flow of the game.
     */

    [SerializeField] float grantInterval;
    [SerializeField] GameObject grantPrefab;
    [SerializeField] Vector2 grantSpawnLocation;

    public static MainGameManager instance = null;

    public Player[] Players = null;
    int[] PlayerScores;
    float grantTimer;
    bool runLevel;
    bool spawnedGrant;
    public int winningCondition = 3;

    public GameObject Victory;
    public Text Timer;
    public AudioSource grantAcquired;
    public AudioSource grantSpawn;

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
                int time = (int)(grantInterval - grantTimer);
                if (time == 0)
                {
                    Timer.text = "Grant has Spawned!";
                }
                else
                {
                    Timer.text = "Grant Spawns in " + time;
                }
            }

            if (grantTimer >= grantInterval)
            {
                SpawnGrant();
                grantTimer = 0.0f;               
            }
        }
    }

    /**
     * Spawns a grant at a specified position
     */
    public void SpawnGrant()
    {
        Debug.Log("Spawning grant!");
        grantSpawn.Play(0);
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
        Debug.Log("scoring index: " + scoringPlayer.ToString());
        if (playerIndex >= 0 && playerIndex < PlayerScores.Length)
        {
            PlayerScores[playerIndex]++;
            Players[playerIndex].grant++;
            //if the score has increased above the winning conditions this means we need to go to the victory screen
            if(PlayerScores[playerIndex] >= winningCondition)
            {
                Victory.GetComponent<Victory>().setText(playerIndex+1);
                Victory.SetActive(true);
            }
            grantAcquired.Play(0);
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
    
}
