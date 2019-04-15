using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grant : MonoBehaviour
{
    [SerializeField] float CaptureTime = 10.0f;

    int[] CapturingTeams;
    int CurrentTimerOwner;
    float Countdown;

    // Start is called before the first frame update
    void Start()
    {
        CapturingTeams = new int[MainGameManager.instance.PlayerCount];
        Countdown = CaptureTime;
        CurrentTimerOwner = 0;

        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }

    // Update is called once per frame
    /**
     * Grant checks every time fixed update is called whether or not the grant has been claimed yet.
     * This is determined by an array that holds ints indicating how many units a player has near the grant.
     * If there is only one player with units nearby, they begin claiming the grant. This timer is paused if
     * there are other players' units in the area and reset if another player is alone with the grant.
     */
    void FixedUpdate()
    {
        int NumCapturing = -1;
        bool MultipleCapturing = false;

        for(int i = 0; i < CapturingTeams.Length; i++)
        {
            //Check if any units are capturing
            if(CapturingTeams[i] > 0)
            {
                //If there are, check if anyone else is doing so
                if (NumCapturing == -1)
                {
                    NumCapturing = i;
                }
                else
                {
                    //If so, set flag
                    MultipleCapturing = true;
                }
            }
        }

        //If multiple people capturing, do nothing
        if (!MultipleCapturing)
        {
            // Check if no one is capturing grant
            if (NumCapturing >= 0)
            {
                //See if capturing player was the same as last check
                if (CurrentTimerOwner == NumCapturing)
                {
                    //Count down timer
                    Countdown -= Time.deltaTime;

                    if (Countdown <= 0.0f)
                    {
                        Debug.Log("Scoring for player " + CurrentTimerOwner.ToString());
                        //Claim Grant
                        MainGameManager.instance.ScoreGrant(CurrentTimerOwner);
                        Destroy(gameObject);
                    }
                }
                else
                {
                    //Set new timer owner and start countdown
                    Countdown = CaptureTime;
                    Countdown -= Time.deltaTime;
                    CurrentTimerOwner = NumCapturing;
                }
            }
            else
            {
                //Reset Timer
                if (Countdown != CaptureTime)
                {
                    Countdown = CaptureTime;
                }
            }
        }

        //Debug.Log("Countdown: " + Countdown.ToString());
    }

    /**
     * When a Unit comes within range of the grant's collision box, it is added to that players' unit count.
     */
    //When Unit collides with grant, add it to the unit counts for its owning player
    void OnTriggerEnter2D(Collider2D other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null)
        {
            Debug.Log(unit.GetOwningPlayerNum().ToString());
            CapturingTeams[unit.GetOwningPlayerNum()]++;
        }
    }

    /**
     * When a Unit leaves the range of the grant's collision box, it is subtracted from that players' unit count.
     */
    //When Unit leaves grant's collision box, remove it from its player's unit count
    void OnTriggerExit1D(Collider2D other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit != null)
        {
            CapturingTeams[unit.GetOwningPlayerNum() - 1]--;
        }
    }
}
