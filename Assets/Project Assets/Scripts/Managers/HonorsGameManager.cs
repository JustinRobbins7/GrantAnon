using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonorsGameManager : MainGameManager
{
    [SerializeField] float minigameInterval;
    [SerializeField] CleanUpGameManager mgOne = null;

    public static HonorsGameManager instanceH = null;

    float minigameTimer;
    bool minigameRunning;

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
        
        if (instanceH == null)
        {
            instanceH = this;
        }
        else if (instanceH != this)
        {
            Destroy(this);
        }

        runLevel = false;
        minigameRunning = false;
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
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].ToggleCamera(false);
                Players[i].DeactivateUnits();
                Players[i].enabled = false;
            }

            mgOne.InitMinigame();

            //minigameRunning = true;
            //minigameTimer = 0.0f;
            runLevel = false;
        }
        else
        {
            Debug.Log("Minigame is null, aborting minigame start.");
        }
    }

    public void CleanUpScore(int playerNumScore, int score)
    {
        mgOne.Score(playerNumScore, score);
    }

    public void OnEndMinigame(List<int> winningPlayers)
    {
        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].ToggleCamera(true);
            Players[i].ReactivateUnits();
            Players[i].enabled = true;
        }

        for (int i = 0; i < winningPlayers.Count; i++)
        {
            Players[winningPlayers[i]].money += mgOne.GetMoneyRewarded();
        }

        minigameRunning = false;
        runLevel = true;
    }
}
