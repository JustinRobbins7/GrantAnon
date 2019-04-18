using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructManager : MonoBehaviour
{
    public GameObject[] menuGameObjs = new GameObject[3];
    public Image[] buttonObj = new Image[2];
    private IndicatorButton[] indicatorScripts;
    public int controllerNum = 0;

    public float waitTime;
    private float timer;

    //button
    private int canvasSelector;

    //Left Stick
    private string horizontalAxis = "";
    private string verticalAxis = "";

    //Right Stick
    private string rHorizontalAxis = "";
    private string rVerticalAxis = "";

    //Symbol Buttons
    private string squareButton = "";
    private string xButton = "";
    private string circleButton = "";
    private string triangleButton = "";

    //L1 & R1
    private string L1 = "";
    private string R1 = "";

    //L2 & R2
    private string L2 = "";
    private string R2 = "";

    //Dpad
    private string DPadX = "";
    private string DPadY = "";

    //L3 & R3
    private string L3 = "";
    private string R3 = "";

    //Misc Buttons
    private string Share = "";
    private string Options = "";
    private string PS = "";
    private string Pad = "";

    private Rigidbody2D rgdbdy2;

    // Start is called before the first frame update

    void Start()
    {
        timer = waitTime;
        canvasSelector = 0;

        //set up which canvas is active
        for (int i = 0; i < menuGameObjs.Length; i++)
        {
            if (i == canvasSelector)
            {
                menuGameObjs[i].SetActive(true);
            }
            else
            {
                menuGameObjs[i].SetActive(false);
            }
        }

        //set up the buttons and their opacities...
        indicatorScripts = new IndicatorButton[buttonObj.Length];
        for (int i = 0; i < buttonObj.Length; i++)
        {
            indicatorScripts[i] = buttonObj[i].GetComponent<IndicatorButton>();
        }

        setIndicators();

        if (controllerNum == 0)
        {
            controllerNum = 1;
        }

        SetControllerNumber(controllerNum);
        rgdbdy2 = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

        //check to see if enough time has passed to read input...
        timer -= Time.deltaTime;
        if (controllerNum > 0 && controllerNum < 5)
        {
            Vector2 movement = new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis));
            //rgdbdy2.position = new Vector2(rgdbdy2.position.x + movement.x, rgdbdy2.position.y + movement.y);

            if (Input.GetButtonDown(circleButton))
            {
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            }
            if (Input.GetAxis(rHorizontalAxis) != 0)
            {
                Debug.Log("R horiz Button Pressed: " + rHorizontalAxis);
                if (timer <= 0)
                {

                    timer = waitTime;
                    if (Input.GetAxis(rHorizontalAxis) > 0)
                    {
                        selectCanvas(1);
                    }
                    else if (Input.GetAxis(rHorizontalAxis) < 0)
                    {
                        selectCanvas(-1);
                    }
                }

            }
            if (Input.GetAxis(horizontalAxis) != 0)
            {
                Debug.Log("horiz Button Pressed: " + horizontalAxis);
                if (timer <= 0)
                {
                    timer = waitTime;
                    if (Input.GetAxis(horizontalAxis) > 0)
                    {
                        selectCanvas(1);
                    }
                    else if (Input.GetAxis(horizontalAxis) < 0)
                    {
                        selectCanvas(-1);
                    }
                }
            }
            if (Input.GetAxis(DPadX) != 0)
            {
                Debug.Log("DPAD Button Pressed: " + Input.GetAxis(DPadX));
                if (timer <= 0)
                {
                    timer = waitTime;
                    selectCanvas((int)Input.GetAxis(DPadX));
                }

            }
        }

    }

    public void SetControllerNumber(int ControllerNum)
    {
        controllerNum = ControllerNum;
        horizontalAxis = "P" + ControllerNum.ToString() + "_Horizontal";
        verticalAxis = "P" + ControllerNum.ToString() + "_Vertical";
        squareButton = "P" + ControllerNum.ToString() + "_Sq";
        xButton = "P" + ControllerNum.ToString() + "_X";
        circleButton = "P" + ControllerNum.ToString() + "_Cir";
        triangleButton = "P" + ControllerNum.ToString() + "_Tri";
        L1 = "P" + ControllerNum.ToString() + "_L1";
        R1 = "P" + ControllerNum.ToString() + "_R1";
        rHorizontalAxis = "P" + ControllerNum.ToString() + "_RHorizontal";
        rVerticalAxis = "P" + ControllerNum.ToString() + "_RVertical";
        L2 = "P" + ControllerNum.ToString() + "_L2";
        R2 = "P" + ControllerNum.ToString() + "_R2";
        DPadX = "P" + ControllerNum.ToString() + "_DPadX";
        DPadY = "P" + ControllerNum.ToString() + "_DPadY";
        L3 = "P" + ControllerNum.ToString() + "_L3";
        R3 = "P" + ControllerNum.ToString() + "_R3";
        Share = "P" + ControllerNum.ToString() + "_Share";
        Options = "P" + ControllerNum.ToString() + "_Options";
        PS = "P" + ControllerNum.ToString() + "_PS";
        Pad = "P" + ControllerNum.ToString() + "_Pad";
    }
    public void setCanvas(int deselect)
    {
        //deselect the old button
        //if deselect =-1 it means it doesn't need to be 
        if (deselect != -1)
        {
            menuGameObjs[deselect].SetActive(false);
        }
        //select the new button
        menuGameObjs[canvasSelector].SetActive(true);
    }
    public void selectCanvas(int i)
    {
        //for moving right or left
        if (i == 1 || i == -1)
        {
            int current = canvasSelector;
            canvasSelector = canvasSelector + i;
            if (canvasSelector < 0)
            {
                //we want nothing to happen....
                canvasSelector = current;
            }
            else if (canvasSelector >= menuGameObjs.Length)
            {
                //we also want nothing to happen
                canvasSelector = current;
            }
            else
            {
                setCanvas(current);
                setIndicators();
            }
        }
    }
    public void setIndicators()
    {
         //set left indicator
        if(canvasSelector == 0) //this means we have no ability to go to the left
        {   
            indicatorScripts[0].onUnselect();
        }else
        {
            indicatorScripts[0].onSelect();
        }
        //set right indicator   
        if (canvasSelector+1 >= menuGameObjs.Length) //this means we have no ability to go to the right
        {
            indicatorScripts[1].onUnselect();
        }
        else
        {
            indicatorScripts[1].onSelect();
        }
    }
}