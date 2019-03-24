using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] Animator anim;
    int breakHash;

    public float arcHeight;

    Collider2D triggerCollider;
    bool thrown;
    Vector3 throwOrigin;
    Vector3 throwCrux;
    Vector3 throwDest;
    float throwDuration;
    float throwTimer;

    void Start()
    {
        breakHash = Animator.StringToHash("Break");
        throwTimer = 0;
        triggerCollider = gameObject.GetComponent<BoxCollider2D>();
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (thrown)
        {
            throwTimer += Time.deltaTime;
            
            if (throwTimer >= throwDuration)
            {
                throwTimer = throwDuration;
            }

            /*
            if (throwTimer / throwDuration >= .55f)
            {
                anim.SetTrigger(breakHash);
            }
            */

            //Debug.Log("Calling Move on Throw arc - t: " + (throwTimer / throwDuration).ToString());
            MoveOnThrowArc(throwTimer/throwDuration);
        }
    }

    public void Throw(Vector3 destination, float throwTime)
    {
        //Init variables
        throwOrigin = gameObject.transform.position;
        throwDest = destination;
        throwDuration = throwTime;
        throwTimer = 0;

        //Calculate third point
        //Calculate midpoint of the line between origin and destination.
        Vector3 midpt = new Vector3((throwOrigin.x + throwDest.x)/2, (throwOrigin.y + throwDest.y)/2, 0);

        //Find line perp. then intersects the midpt
        float slope = (throwDest.y - throwOrigin.y) / (throwDest.x - throwOrigin.x);
        float perpSlope = -1 / slope;
        float b = midpt.y - (perpSlope * midpt.x);

        //Calculate point whose y is arcHeight above midpoint and set as second arc point
        float yCrux = midpt.y + arcHeight;
        float xCrux = (yCrux - b) / perpSlope;

        throwCrux = new Vector3(xCrux, yCrux, 0);

        //Debug.Log("Throwing potion - Start: " + throwOrigin.ToString() + " - Crux: " + throwCrux.ToString() + " - End: " + throwDest.ToString());

        //Begin throw
        thrown = true;
    }

    void MoveOnThrowArc(float t)
    {
        //Debug.Log("Arcing - t: " + t.ToString());
        if (thrown)
        {
            
            if (t != 1)
            {
                float revT = 1 - t;
                float revTSq = revT * revT;
                float tSq = t * t;

                //Equation for Quadratic Bezier Curve: (1-t)^2 * startVect + 2*(1-t)*t * MidVect + t^2 * endVect ; 0 <= t <= 1
                Vector3 newPos = revTSq*throwOrigin + 2*revT*t*throwCrux + tSq*throwDest;
                gameObject.transform.position = newPos;
            }
            else
            {
                //Debug.Log("Throw Complete!");
                //Put object at destination
                anim.SetTrigger(breakHash);

                thrown = false;
                gameObject.transform.position = throwDest;

                //Activate collider to allow pickup
                if (triggerCollider != null)
                {
                    triggerCollider.enabled = true;
                }

                //Reset variables
                throwOrigin = new Vector3(0, 0, 0);
                throwDest = new Vector3(0, 0, 0);
                throwDuration = 0;
                throwTimer = 0;
            }
            
        }
        else
        {
            return;
        }
    }
}
