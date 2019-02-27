using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SavedLevel
{
    public string levelName;

    public int numberOfPlayers;

    public PlayerController[] Players;

    public float[] playerScores;


}
