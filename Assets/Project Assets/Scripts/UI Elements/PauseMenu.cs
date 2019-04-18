using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject start;
    public GameObject timer;
    bool visibility;
    bool check;
    // Start is called before the first frame update
    void Start()
    {
        visibility = false;
        check = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!check)
        {
            if (start != null)
            {
                if (!start.activeSelf)
                {
                    check = true;
                    timer.SetActive(true);
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("PAll_Start"))
            {
                    toggle();
            }

            if (visibility)
            {
                if (Input.GetButtonDown("PAll_X"))
                {
                    toggle();
                }
                if (Input.GetButtonDown("PAll_Cir"))
                {
                    Time.timeScale = 1;
                    SceneManager.LoadScene("Menu", LoadSceneMode.Single);
                }
            }
        }
    }

    public void toggle()
    {
        visibility = !menu.activeSelf;
        //Debug.LogError("We toggle");
        menu.SetActive(visibility);
        if (visibility)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
