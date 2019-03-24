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
    [SerializeField] float roundTime;
    [SerializeField] CleanUpGame_UIManager ui;
    bool runMinigame;
    bool endingMinigame;
    float roundTimer;

    [HideInInspector] public List<Sweeper> players = null;
    MessMonster currentMonster = null;

    List<int> mgScores;

    // Start is called before the first frame update
    void Start()
    {
        runMinigame = false;
        endingMinigame = false;
        roundTimer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (runMinigame)
        {
            roundTimer += Time.deltaTime;

            if (roundTimer >= roundTime && !endingMinigame)
            {
                endingMinigame = true;
                runMinigame = false;
                InitEndMinigame();
            }
        }
    }

    public void InitMinigame()
    {
        ToggleCamera(true);
        ui.LoadCleanUpStart();
    }

    public void InitEndMinigame()
    {
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

        ui.LoadCleanUpEnd();
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

        Debug.Log("mgScores Count Before Add: " + mgScores.Count.ToString());

        for (int i = 0; i < NumPlayers; i++)
        {
            mgScores.Add(0);
        }

        Debug.Log("mgScores Count After Add: " + mgScores.Count.ToString());

        runMinigame = true;
    }

    public void EndMinigame()
    {
        //Turn off camera
        ToggleCamera(false);

        List<int> winningPlayers = new List<int>();
        int bestScore = -1;

        if (mgScores == null)
        {
            Debug.Log("mgScores is null!");
            return; 
        }

        Debug.Log("mgScores Count Before Score Tally: " + mgScores.Count.ToString());
        Debug.Log("mgScores first value before score tally: " + mgScores[0]);

        for (int i = 0; i < mgScores.Count; i++)
        {
            Debug.Log("i: " + i.ToString());
            
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

        //Reset Scores
        mgScores = null;
        
        if (HonorsGameManager.instanceH != null)
        {
            HonorsGameManager.instanceH.OnEndMinigame(winningPlayers);
        }
        else
        {
            Debug.Log("Not using Honors Game Manager!");
        }

        roundTimer = 0;
        endingMinigame = false;
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

    public int GetMoneyRewarded()
    {
        return moneyRewarded;
    }
}
