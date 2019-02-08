using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    public float cameraSpeed = 0.1f;

    public int controllerNum = 0;

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
        rgdbdy2 = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (controllerNum > 0 && controllerNum < 5)
        {
            Vector2 movement = new Vector2(Input.GetAxis(horizontalAxis) * cameraSpeed, Input.GetAxis(verticalAxis) * cameraSpeed);
            rgdbdy2.position = new Vector2(rgdbdy2.position.x + movement.x, rgdbdy2.position.y + movement.y);

            if (Input.GetButtonDown(squareButton))
            {
                Debug.Log("Button Pressed: " + squareButton);
            }

            if (Input.GetButtonDown(xButton))
            {
                Debug.Log("Button Pressed: " + xButton);
            }

            if (Input.GetButtonDown(circleButton))
            {
                Debug.Log("Button Pressed: " + circleButton);
            }

            if (Input.GetButtonDown(triangleButton))
            {
                Debug.Log("Button Pressed: " + triangleButton);
            }

            if (Input.GetButtonDown(L1))
            {
                Debug.Log("Button Pressed: " + L1);
            }

            if (Input.GetButtonDown(R1))
            {
                Debug.Log("Button Pressed: " + R1);
            }

            if (Input.GetAxis(L2) != -1)
            {
               // Debug.Log("Player " + controllerNum.ToString() + " - L2: " + Input.GetAxis(L2).ToString());
            }

            if (Input.GetAxis(R2) != -1)
            {
                //Debug.Log("Player " + controllerNum.ToString() + " - R2: " + Input.GetAxis(R2).ToString());
            }

            if (Input.GetAxis(rHorizontalAxis) != 0)
            {
                Debug.Log("rHorizontalAxis: " + Input.GetAxis(rHorizontalAxis).ToString());
            }

            if (Input.GetAxis(rVerticalAxis) != 0)
            {
                Debug.Log("rVerticalAxis: " + Input.GetAxis(rVerticalAxis).ToString());
            }

            if (Input.GetAxis(DPadX) != 0)
            {
                Debug.Log("DPadX: " + Input.GetAxis(DPadX).ToString());
            }

            if (Input.GetAxis(DPadY) != 0)
            {
                Debug.Log("DPadY: " + Input.GetAxis(DPadY).ToString());
            }

            if (Input.GetButtonDown(L3))
            {
                Debug.Log("Button Pressed: " + L3);
            }

            if (Input.GetButtonDown(R3))
            {
                Debug.Log("Button Pressed: " + R3);
            }

            if (Input.GetButtonDown(Share))
            {
                Debug.Log("Button Pressed: " + Share);
            }

            if (Input.GetButtonDown(Options))
            {
                Debug.Log("Button Pressed: " + Options);
            }

            if (Input.GetButtonDown(PS))
            {
                Debug.Log("Button Pressed: " + PS);
            }

            if (Input.GetButtonDown(Pad))
            {
                Debug.Log("Button Pressed: " + Pad);
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

    public void SetCameraViewport(int NumberOfPlayers)
    {
        Camera playerCam = gameObject.GetComponentInChildren<Camera>();

        if (playerCam == null)
        {
            Debug.Log("ERROR: Player Camera Component Not Found For Player " + controllerNum.ToString() + "!");
            return;
        }

        Rect newViewport = new Rect(0,0,0,0);
        if (NumberOfPlayers > 0 && NumberOfPlayers <= 2)
        {
            switch (controllerNum)
            {
                case 1:
                    newViewport = new Rect(0, 0.5f, 1.0f, 0.5f);
                    break;
                case 2:
                    newViewport = new Rect(0, 0, 1.0f, 0.5f);
                    break;
                default:
                    Debug.Log("ERROR: SetViewport - Invalid controller number " + controllerNum.ToString() + " for number of players: " + NumberOfPlayers.ToString());
                    return;
            }
        }
        else if (NumberOfPlayers > 0 && NumberOfPlayers <= 4)
        {
            switch (controllerNum)
            {
                case 1:
                    newViewport = new Rect(0, 0.5f, 0.5f, 0.5f);
                    break;
                case 2:
                    newViewport = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                    break;
                case 3:
                    newViewport = new Rect(0, 0, 0.5f, 0.5f);
                    break;
                case 4:
                    newViewport = new Rect(0.5f, 0, 0.5f, 0.5f);
                    break;
                default:
                    Debug.Log("ERROR: SetViewport - Invalid controller number " + controllerNum.ToString() + " for number of players: " + NumberOfPlayers.ToString());
                    return;
            }
        }

        playerCam.rect = newViewport;
    }
}
