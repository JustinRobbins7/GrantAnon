using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    bool visibility;
    bool first;
    // Start is called before the first frame update
    void Start()
    {
        visibility = false;
        first = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("PAll_Start"))
        {
            if (first)
            {
                first = false;
            }
            else
            {
                toggle();
            }
        }

        if (visibility) {
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
