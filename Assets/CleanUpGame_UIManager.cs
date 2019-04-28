using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleanUpGame_UIManager : MonoBehaviour
{
    [SerializeField] GameObject startScreen;
    [SerializeField] GameObject endScreen;

    CleanUpGameManager owner;
    PauseMenu pause = null;

    enum Mode{
        Inactive,
        Start,
        End
    };

    Mode currentMode;

    // Start is called before the first frame update
    void Start()
    {
        owner = FindObjectOfType<CleanUpGameManager>();
        Deactivate();
        pause = FindObjectOfType<PauseMenu>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentMode == Mode.Inactive)
        {

        }
        else if (currentMode == Mode.Start)
        {
            if (Input.GetButtonDown("PAll_Pad"))
            {
                Debug.Log("Start Minigame!");

                if (MainGameManager.instance != null)
                {
                    owner.StartMinigame(MainGameManager.instance.GetNumPlayers());
                }
                else
                {
                    Debug.Log("ERROR: No MainGameManager instance!");
                }

                Deactivate();
            }
        }
        else if (currentMode == Mode.End)
        {
            if (Input.GetButtonDown("PAll_Pad"))
            {
                Debug.Log("End Minigame!");

                if (pause != null)
                {
                    pause.toggleTimer();
                }

                owner.EndMinigame();

                Deactivate();
            }
        }
    }

    public void LoadCleanUpStart()
    {
        currentMode = Mode.Start;

        startScreen.SetActive(true);

        if(pause != null)
        {
            pause.toggleTimer();
        }
    }

    public void LoadCleanUpEnd()
    {
        currentMode = Mode.End;

        endScreen.SetActive(true);
    }

    public void Deactivate()
    {
        currentMode = Mode.Inactive;

        startScreen.SetActive(false);
        endScreen.SetActive(false);
    }
}
