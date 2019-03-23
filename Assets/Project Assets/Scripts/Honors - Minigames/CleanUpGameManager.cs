using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUpGameManager : MonoBehaviour
{
    [SerializeField] Camera minigameCamera;
    [SerializeField] MessMonster monsterPrefab;
    [SerializeField] Vector3[] playerSweeperSpawns;
    [SerializeField] Sweeper[] playerSweeperOptions;
    [SerializeField] Vector3 MonsterSpawn;
    [SerializeField] int moneyRewarded;

    [HideInInspector] public List<Sweeper> players = null;
    MessMonster currentMonster = null;

    List<int> mgScores;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    public void ResetMinigame()
    {
        //Reset Scores
        mgScores = null;

        if (players != null)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] != null)
                {
                    Destroy(players[i].gameObject);
                }
            }

            players.Clear();
        }

        if (currentMonster != null)
        {
            Destroy(currentMonster.gameObject);
        }

        //Turn off camera
        ToggleCamera(false);
    }

    public void StartMinigame(int NumPlayers)
    {
        Debug.Log("Initializing Sweepers");

        players = new List<Sweeper>(NumPlayers);

        for (int i = 0; i < NumPlayers; i++)
        {
            players.Add(Instantiate<Sweeper>(playerSweeperOptions[i]));
            players[i].SetControllerNumber(i, MainGameManager.instance.GetPlayerControllerZeroBased(i));
            players[i].InitSweeper(playerSweeperSpawns[i]);
        }

        currentMonster = Instantiate<MessMonster>(monsterPrefab);
        currentMonster.transform.position = MonsterSpawn;

        mgScores = new List<int>(NumPlayers);
        for (int i = 0; i < NumPlayers; i++)
        {
            mgScores.Add(0);
        }

        ToggleCamera(true);
    }

    public void EndMinigame()
    {
        List<int> winningPlayers = new List<int>();
        int bestScore = -1;

        for (int i = 0; i < mgScores.Count; i++)
        {
            if (mgScores[i] > bestScore)
            {
                winningPlayers.Clear();
                winningPlayers.Add(i);
                bestScore = mgScores[i];
            }
            else if (mgScores[i] == bestScore)
            {
                winningPlayers.Add(i);
            }
        }

        ResetMinigame();
        
        if (HonorsGameManager.instanceH != null)
        {
            HonorsGameManager.instanceH.OnEndMinigame(winningPlayers);
        }
        else
        {
            Debug.Log("Not using Honors Game Manager!");
        }
    }

    public void Score(int numPlayer, int score)
    {
        if (numPlayer < mgScores.Count)
        {
            mgScores[numPlayer] += score;
        }
    }

    public void ToggleCamera(bool activate)
    {
        if (activate)
        {
            minigameCamera.enabled = true;
        }
        else
        {
            minigameCamera.enabled = false;
        }
    }
}
