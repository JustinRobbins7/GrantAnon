using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameInfo : MonoBehaviour
{
    [SerializeField] Camera minigameCamera;
    [SerializeField] MessMonster monster;

    public Vector3[] playerSweeperSpawns;
    public Sweeper[] playerSweeperOptions;

    [HideInInspector] public List<Sweeper> players;

    List<int> mgScores;

    // Start is called before the first frame update
    void Start()
    {
        players = new List<Sweeper>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetMinigame()
    {
        //Reset Scores
        mgScores = null;

        //Turn off camera
        ToggleCamera(false);
    }

    public void StartMinigame(int NumPlayers)
    {
        mgScores = new List<int>(NumPlayers);

        ToggleCamera(true);
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
