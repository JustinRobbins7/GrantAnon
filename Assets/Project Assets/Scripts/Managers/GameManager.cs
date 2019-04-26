using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Parent class for other GameManagers.
 */
public abstract class GameManager : MonoBehaviour
{
    /**
     * Abstract class for the GameManagers, requires implementation of StartLevel and the tracking of a number of players.
     */
    [HideInInspector] public int PlayerCount;

    public abstract void StartLevel(int startPlayerCount);
    
}
