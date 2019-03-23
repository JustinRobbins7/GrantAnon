using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Main Level
    [SerializeField] float cameraSpeed = 10f;
    [SerializeField] int controllerNum;

    [SerializeField] GameObject PlayerUnit = null;
    List<GameObject> units = null;

    [SerializeField] GameObject BuildingOne = null;
    List<GameObject> incomeBuildings = null;

    [HideInInspector] public GameObject BuildingRoot = null;
    [HideInInspector] public GameObject UnitRoot = null;
    [HideInInspector] public int money;

    bool unitsactive;

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
    void Start()
    {
        SetControllerNumber(controllerNum);

        units = new List<GameObject>();
        incomeBuildings = new List<GameObject>();

        money = 0;
        unitsactive = true;
    }

    void FixedUpdate()
    {
        MainGameInputCheck();

        /*
        else if (levelMode == 1)
        {
            CleanUpMinigameInputCheck();
        }
        */
    }

    private void MainGameInputCheck()
    {
        if (controllerNum > 0 && controllerNum < 5)
        {
            //Move Camera
            if (Input.GetAxis(horizontalAxis) != 0 || Input.GetAxis(verticalAxis) != 0)
            {
                updateCameraLocation();
            }

            //Spawn Building
            if (Input.GetButtonDown(squareButton))
            {
                if (BuildingRoot != null && BuildingOne != null)
                {
                    GameObject SpawnedBuilding = Instantiate(BuildingOne);
                    SpawnedBuilding.transform.parent = BuildingRoot.transform;
                    SpawnedBuilding.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
                    SpawnedBuilding.layer = SortingLayer.GetLayerValueFromName("Foreground");
                    if (SpawnedBuilding.GetComponent<IncomeBuilding>())
                    {
                        // Debug.Log("Controller Number: " + controllerNum.ToString());
                        SpawnedBuilding.GetComponent<IncomeBuilding>().OwningPlayerNum = controllerNum;
                    }
                    else
                    {
                        //Debug.Log("Could not find IncomeBuilding Component!");
                    }
                    incomeBuildings.Add(SpawnedBuilding);
                }
            }

            //Spawn Unit
            if (Input.GetButtonDown(xButton))
            {
                if (UnitRoot != null && PlayerUnit != null)
                {
                    GameObject SpawnedUnit = Instantiate(PlayerUnit);
                    SpawnedUnit.transform.parent = UnitRoot.transform;
                    SpawnedUnit.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
                    SpawnedUnit.layer = SortingLayer.GetLayerValueFromName("Characters");
                    if (SpawnedUnit.GetComponent<Unit>())
                    {
                        SpawnedUnit.GetComponent<Unit>().OwningControllerNum = controllerNum;
                    }
                    units.Add(SpawnedUnit);
                }
            }

            //Move Selected Units
            if (Input.GetButtonDown(circleButton))
            {
                foreach (var selectableObject in FindObjectsOfType<Unit>())
                {
                    if (selectableObject.OwningControllerNum == controllerNum && selectableObject.IsSelected())
                    { // Ensure that unit is in same group as this camera
                        selectableObject.GetComponent<Unit>().Move(transform.position);
                    }
                }
            }

            //Activate Selector
            if (Input.GetButtonDown(L3))
            {
                // If you are not currently in the selection phase (and want to switch to it), then deactivate all selectable gameObjects for the player first
                if (!isSelecting)
                {
                    foreach (var selectableObject in FindObjectsOfType<Unit>())
                    {
                        if (selectableObject.OwningControllerNum == controllerNum)
                        { // Ensure that unit is in same group as this camera
                            selectableObject.SetSelected(false);
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

            if (Input.GetButtonDown(Pad))
            {
                if (unitsactive)
                {
                    Debug.Log("Deactivating Units and Buildings");
                    DeactivateUnits();
                }
                else
                {
                    Debug.Log("Reactivating Units and Buildings");
                    ReactivateUnits();
                }
            }

            UpdateSelectionCircle();
        }
    }

    /*
    private void CleanUpMinigameInputCheck()
    {
        if (controllerNum > 0 && controllerNum < 5)
        {
            if (Input.GetAxis(horizontalAxis) != 0 || Input.GetAxis(verticalAxis) != 0)
            {
                if (sweeper != null)
                {
                    Vector3 sweeperPos = sweeper.transform.position;
                    sweeperPos.x += Input.GetAxis(horizontalAxis) * Time.deltaTime;
                    sweeperPos.y += Input.GetAxis(verticalAxis) * Time.deltaTime;
                    sweeper.transform.position = sweeperPos;
                }
                else
                {
                    Debug.Log("No Sweeper!");
                }
            }
        }
    }
    */

    private void updateCameraLocation() {
        Vector3 position = transform.position;
        position.x += Input.GetAxis(horizontalAxis) * cameraSpeed * Time.deltaTime;
        position.y += Input.GetAxis(verticalAxis) * cameraSpeed * Time.deltaTime;
        transform.position = position;

        gameObject.UpdateCircleDraw(radius);
    }

    private void UpdateSelectionCircle() {
        if (isSelecting) {
            foreach (var selectableObject in FindObjectsOfType<Unit>()) {
                if (selectableObject.GetComponent<Unit>().OwningControllerNum == controllerNum && IsWithinBounds(selectableObject.gameObject)) {
                    selectableObject.GetComponent<Unit>().SetSelected(true);
                }
            }
        }
    }

    private bool IsWithinBounds(GameObject gameObject) {
        if (!isSelecting) {
            return false;
        }

        Vector3 adjustedCameraPos = this.gameObject.transform.position;
        adjustedCameraPos.z = 0;

        // Return true if the distance between the selectableObject and the camera is less than the radius
        return Vector3.Distance(gameObject.transform.position, adjustedCameraPos) < radius;
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

    public void ToggleCamera(bool activate)
    {
        Camera cam = GetComponent<Camera>();

        if (activate)
        {
            cam.enabled = true;
        }
        else
        {
            cam.enabled = false;
        }
    }
    
    public void DeactivateUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].SetActive(false);
        }

        for (int i = 0; i < incomeBuildings.Count; i++)
        {
            incomeBuildings[i].SetActive(false);
        }

        unitsactive = false;
    }

    public void ReactivateUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].SetActive(true);
        }

        for (int i = 0; i < incomeBuildings.Count; i++)
        {
            incomeBuildings[i].SetActive(true);
        }

        unitsactive = true;
    }

    /*
    public void SetGameMode(int GameModeID)
    {
        if(GameModeID == 0)
        {
            //Main Game
            if (sweeper != null)
            {
                sweeper.SetActive(false);
            }

            levelMode = 0;
        }
        else if(GameModeID == 1)
        {
            //CleanUp
            levelMode = 1;
        }
    }
    */
}
