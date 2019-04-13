using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameStarter : MonoBehaviour
{
    public Text minigameTimerClock;
    [SerializeField] float minigameInterval;
    float minigameTimer;
    bool minigameRunning;
    CircleCollider2D activator = null;

    // Start is called before the first frame update
    void Start()
    {
        minigameTimer = minigameInterval;
        minigameRunning = false;
        activator = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activator != null && !(activator.enabled) && !minigameRunning)
        {
            minigameTimer -= Time.deltaTime;
            if (minigameTimerClock != null)
            {
                int minutes = Mathf.FloorToInt(minigameTimer / 60);
                int seconds = Mathf.FloorToInt(minigameTimer % 60);
                minigameTimerClock.text = minutes.ToString() + ":" + seconds.ToString();
            }

            if (minigameTimer <= 0)
            {
                if (activator != null)
                {
                    activator.enabled = true;
                    if (minigameTimerClock != null)
                    {
                        minigameTimerClock.text = "The researchers need help, come quickly!";
                    }
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(!minigameRunning && other.gameObject.GetComponent<Unit>() != null)
        {
            minigameRunning = true;
            //Start minigame
            activator.enabled = false;
        }
    }

    public void restartTimer()
    {
        minigameRunning = false;
        minigameTimer = minigameInterval;
    }
}
