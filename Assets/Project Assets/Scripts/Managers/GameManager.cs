using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : MonoBehaviour
{
    /**
     * Abstract class for the GameManagers, requires implementation of StartLevel and StartLevelAfterLoad.
     */

    [HideInInspector] public int PlayerCount;

    public abstract void StartLevel(int startPlayerCount);
}
