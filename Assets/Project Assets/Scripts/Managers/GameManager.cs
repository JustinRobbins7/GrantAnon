using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : MonoBehaviour
{
    [HideInInspector] public int PlayerCount;

    public abstract void StartLevel(int startPlayerCount);

    public abstract void StartLevelAfterLoad(int startPlayerCount);
}
