using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonorsGameManager : MainGameManager
{
    [SerializeField] float minigameInterval;
    [SerializeField] MinigameInfo mgOne = null;

    float minigameTimer;
    bool minigameRunning;

    bool minigameInitialized;

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
        minigameRunning = false;
        minigameInitialized = false;
        zeroBasedPlayerToController = new Dictionary<int, int>();
    }

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

            if (!minigameRunning)
            {
                minigameTimer += Time.deltaTime;
            }

            if (minigameTimer >= minigameInterval)
            {
                minigameRunning = true;
                minigameTimer = 0.0f;
                StartMinigame();
            }
        }
    }

    void StartMinigame()
    {
        if (mgOne != null)
        {
            if (!minigameInitialized)
            {
                for (int i = 0; i < Players.Length; i++)
                {
                    Sweeper spawnedSweeper = Instantiate<Sweeper>(mgOne.playerSweeperOptions[i]);
                    spawnedSweeper.SetControllerNumber(zeroBasedPlayerToController[i]);
                    spawnedSweeper.ResetSweeper(mgOne.playerSweeperSpawns[i]);
                    mgOne.players.Add(spawnedSweeper);
                }

                minigameInitialized = true;
            }

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].ToggleCamera(false);
                Players[i].DeactivateUnits();
                Players[i].enabled = false;
            }

            mgOne.ResetMinigame();
            mgOne.StartMinigame(Players.Length);

            //minigameRunning = true;
            //minigameTimer = 0.0f;
            runLevel = false;
        }
        else
        {
            Debug.Log("Minigame is null, aborting minigame start.");
        }
    }

    public void EndMinigame()
    {
        
    }
}
