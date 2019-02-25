using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image[] menuGameObjs = new Image[3];
    private MenuButton[] menuScripts;
    public int controllerNum = 0;

    public float waitTime;
    private float timer;

    //button
    private int buttonSelector;

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
        menuScripts = new MenuButton[menuGameObjs.Length];
        for (int i = 0; i < menuGameObjs.Length; i++)
        {

            if (menuGameObjs[i].GetComponent(typeof(StartButton)))
            {
                menuScripts[i] = menuGameObjs[i].GetComponent<StartButton>();
            }
            else if (menuGameObjs[i].GetComponent(typeof(InstructionButton)))
            {
                menuScripts[i] = menuGameObjs[i].GetComponent<InstructionButton>();
            }
            else if (menuGameObjs[i].GetComponent(typeof(ExitButton)))
            {
                menuScripts[i] = menuGameObjs[i].GetComponent<ExitButton>();
            }
            else
            {
                menuScripts[i] = null;
            }

        }

        buttonSelector = 0;
        highlightButton(-1);
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
            
            if (Input.GetButtonDown(xButton))
            {
                menuScripts[buttonSelector].onClick();
            }
            if (Input.GetAxis(rVerticalAxis) != 0)
            {
                if (timer <= 0)
                {
                    timer = waitTime;
                    if (Input.GetAxis(rVerticalAxis) > 0)
                    {

                        selectButton(1);
                    }
                    else if (Input.GetAxis(rVerticalAxis) < 0)
                    {
                        selectButton(-1);
                    }
                }

            }
            if (Input.GetAxis(verticalAxis) != 0)
            {
                if (timer <= 0)
                {
                    timer = waitTime;
                    if (Input.GetAxis(verticalAxis) > 0)
                    {
                        selectButton(1);
                    }
                    else if (Input.GetAxis(verticalAxis) < 0)
                    {
                        selectButton(-1);
                    }
                }
            }
            if (Input.GetAxis(DPadY) != 0)
            {
                if (timer <= 0)
                {
                    timer = waitTime;
                    selectButton((int)Input.GetAxis(DPadY));
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

    public void highlightButton(int deselect)
    {
        //deselect the old button
        //if deselect =-1 it means it doesn't need to be 
        if (deselect != -1)
        {
            if (menuScripts[deselect] != null)
            {
                menuScripts[deselect].onUnselect();
            }
        }
        //select the new button
        if (menuScripts[buttonSelector] != null)
        {
            menuScripts[buttonSelector].onSelect();
        }
    }
    public void selectButton(int i)
    {
        //for moving up or down
        if (i == 1 || i == -1)
        {
            int current = buttonSelector;
            buttonSelector = buttonSelector - i;
            if (buttonSelector < 0)
            {
                buttonSelector = menuGameObjs.Length - 1;
            }
            else if (buttonSelector >= menuGameObjs.Length)
            {
                buttonSelector = 0;
            }
            highlightButton(current);
        }
    }
}
