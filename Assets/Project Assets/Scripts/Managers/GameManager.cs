using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : MonoBehaviour
{
    public abstract void StartLevel();

    public abstract void StartLevelAfterLoad();
}
