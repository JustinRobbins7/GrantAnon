using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /**
     * Main player script, manages which input axes this player reads from 
     * and performs defined behaviors based on those axes inputs.
     */
    [SerializeField] float cameraSpeed = 10f;
    public int controllerNum;
    public Vector2 bottomLeftLimit;
    public Vector2 topRightLimit;

    private Player player;

    // Variables for drawing the unit selection circle
    private bool isSelecting = false;
    private float radius = 2f;

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


    // Start is called before the first frame update
    /**
     * Sets initial information for circle draw radius and sets unusable controller num to avoid crashes.
     * Actual number set in PlayerControllerAssigner
     */

    void Awake()
    {
        player = GetComponent<Player>();
    }

    /**
     * Reads controller axes to perform a number of actions.
     */
    void FixedUpdate()
    {
        if (controllerNum > 0 && controllerNum < 5)
        {
            /**
             * Moves camera
             */
            if (Input.GetAxis(horizontalAxis) != 0 || Input.GetAxis(verticalAxis) != 0)
            {
                updateCameraLocation();
            }

            /**
             * Spawns building, placing it under this player's control
             */
            if (Input.GetButtonDown(squareButton))
            {
                player.SpawnBuilding(transform.position);
            }


            /**
             * Draws a selection circle to select units
             */
            if (Input.GetButtonDown(xButton))
            {
                // If you are not currently in the selection phase (and want to switch to it), then deactivate all selectable gameObjects for the player first
                if (!isSelecting)
                {
                    /*
                    foreach (var unit in player.GetUnits())
                    {
                        unit.SetSelected(false);
                    }
                    */

                    for (int i = 0; i < player.units.Count; i++)
                    {
                        if (player.units[i] != null)
                        {
                            player.units[i].GetComponent<Unit>().SetSelected(false);
                        }
                    }

                    // Initialize the line renderer
                    gameObject.CreateCircleDraw(radius);
                }
                else
                {
                    gameObject.DestroyCircleDraw();
                }
                isSelecting = !isSelecting;
            }

            /**
            * Instructs the currently selected units to move the current location of the camera.
            */
            if (Input.GetButtonDown(circleButton))
            {
                /*
                Unit[] selectedUnits = Array.FindAll(player.GetUnits(), unit => unit.IsSelected());
                foreach (var unit in selectedUnits)
                {
                    unit.Move(transform.position);
                }
                */

                for (int i = 0; i < player.units.Count; i++)
                {
                    if (player.units[i] != null)
                    {
                        Unit unit = player.units[i].GetComponent<Unit>();
                        if (unit.IsSelected())
                        {
                            unit.Move(transform.position);
                        }
                    }
                }
            }
            /**
            * Spawns unit, placing it under this player's control
            */
            if (Input.GetButtonDown(triangleButton))
            {
                // player.SpawnUnit(transform.position);
                player.SpawnUnitAtBase();
            }

            if (Input.GetButtonDown(L1))
            {
            }

            if (Input.GetButtonDown(R1))
            {
            }

            if (Input.GetAxis(L2) != -1)
            {
            }

            if (Input.GetAxis(R2) != -1)
            {
            }

            if (Input.GetAxis(rHorizontalAxis) != 0)
            {
            }

            if (Input.GetAxis(rVerticalAxis) != 0)
            {
            }

            if (Input.GetAxis(DPadX) != 0)
            {
            }

            if (Input.GetAxis(DPadY) != 0)
            {
            }

            if (Input.GetButtonDown(L3))
            {
            }

            if (Input.GetButtonDown(R3))
            {
            }

            if (Input.GetButtonDown(Share))
            {
            }

            if (Input.GetButtonDown(Options))
            {
                //here's where we add the pause button
                //Debug.LogError("Options Button Pressed!");
                this.GetComponent<PauseMenu>().toggle();
            }

            if (Input.GetButtonDown(PS))
            {
            }

            if (Input.GetButtonDown(Pad))
            {
            }

            UpdateSelectionCircle();
        }
    }

    /**
     * Moves camera based on this gameobject's location.
     */
    private void updateCameraLocation()
    {
        Vector3 position = transform.position;
        position.x += Input.GetAxis(horizontalAxis) * cameraSpeed * Time.deltaTime;
        position.y += Input.GetAxis(verticalAxis) * cameraSpeed * Time.deltaTime;

        if (position.x > topRightLimit.x)
        {
            position.x = topRightLimit.x;
        }

        if (position.x < bottomLeftLimit.x)
        {
            position.x = bottomLeftLimit.x;
        }

        if (position.y > topRightLimit.y)
        {
            position.y = topRightLimit.y;
        }

        if (position.y < bottomLeftLimit.y)
        {
            position.y = bottomLeftLimit.y;
        }

        transform.position = position;

        gameObject.UpdateCircleDraw(radius);
    }

    /**
     * Moves drawn circle on this gameobject's location.
     */
    private void UpdateSelectionCircle()
    {
        if (isSelecting)
        {
            /*
            foreach (var selectableObject in FindObjectsOfType<Unit>())
            {
                Unit[] unitsInBounds = Array.FindAll(player.GetUnits(), unit => IsWithinBounds(unit.gameObject));
                foreach (var unit in unitsInBounds)
                {
                    unit.SetSelected(true);
                }
            }
            */
            
            for (int i = 0; i < player.units.Count; i++)
            {
                if (player.units[i] != null)
                {
                    Unit unit = player.units[i].GetComponent<Unit>();
                    if (IsWithinBounds(unit.gameObject))
                    {
                        unit.SetSelected(true);
                    }
                }
            }
            
        }
    }

    /**
     * Checks if selectable object is within bounds of the drawn circle
     */
    private bool IsWithinBounds(GameObject gameObject)
    {
        if (!isSelecting)
        {
            return false;
        }

        Vector3 adjustedCameraPos = this.gameObject.transform.position;
        adjustedCameraPos.z = 0;

        // Return true if the distance between the selectableObject and the camera is less than the radius
        return Vector3.Distance(gameObject.transform.position, adjustedCameraPos) < radius;
    }

    /**
     * Sets the names of the axes this controller is listening for based on the given player number 
     * as well as setting this player controller's player number to the given number.
     */
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

    /**
     * Defines which section of the screen this player's camera occupies, 
     * partitioning the screen into sections based on the total number of players.
     * This method sets the camera location on screen based on the player number and
     * total number of playeres given.
     */
    public void SetCameraViewport(int ViewportID, int NumberOfPlayers)
    {
        Camera playerCam = gameObject.GetComponentInChildren<Camera>();

        if (playerCam == null)
        {
            Debug.Log("ERROR: SetCameraViewport - Player Camera Component Not Found For Player " + controllerNum.ToString() + "!");
            return;
        }

        if (ViewportID < 1 || ViewportID > NumberOfPlayers)
        {
            Debug.Log("ERROR: SetCameraViewport - Invalid viewport ID for number of players given. Viewport ID: " + ViewportID.ToString() + " | Number of Players: " + NumberOfPlayers.ToString());
            return;
        }

        Rect newViewport = new Rect(0, 0, 0, 0);
        if (NumberOfPlayers > 0 && NumberOfPlayers <= 2)
        {
            switch (ViewportID)
            {
                case 1:
                    newViewport = new Rect(0, 0.5f, 1.0f, 0.5f);
                    break;
                case 2:
                    newViewport = new Rect(0, 0, 1.0f, 0.5f);
                    break;
                default:
                    return;
            }
        }
        else if (NumberOfPlayers > 0 && NumberOfPlayers <= 4)
        {
            switch (ViewportID)
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
                    return;
            }
        }

        playerCam.rect = newViewport;
    }
}
