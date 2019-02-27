using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : GameManager
{
    public PlayerController[] Players = null;
    public int[] PlayerScores;
    public float grantInterval;

    public static MainGameManager instance = null;

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

    public override void StartLevel()
    {
        grantTimer = 0.0f;

        spawnedGrant = false;
        runLevel = true;
    }

    public override void StartLevelAfterLoad()
    {

    }
}
