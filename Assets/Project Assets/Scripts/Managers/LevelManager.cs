using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/**
 * Class used to load scenes in levels
 */
public class LevelManager : MonoBehaviour
{
    /**
     * Loads a scene given a scene name
     */
    public void LoadLevel(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }
}
