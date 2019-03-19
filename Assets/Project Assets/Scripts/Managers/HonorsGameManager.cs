using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonorsGameManager : MainGameManager
{
    [SerializeField] float minigameInterval;
    [SerializeField] Camera minigameCamera;

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

        runLevel = false;
        minigameRunning = false;
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
                StartMinigame();
                minigameTimer = 0.0f;
            }
        }
    }

    void StartMinigame()
    {
        minigameRunning = true;
    }
}
