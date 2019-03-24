using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sweeper : MonoBehaviour
{
    public float speed;
    int carriedPotions;
    int controllerNum;
    int playerNum;
    [SerializeField] Text scoreText;

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
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxis(horizontalAxis) != 0 || Input.GetAxis(verticalAxis) != 0)
        {
            //Debug.Log("Input Detected!");
            Vector3 sweeperPos = transform.position;
            sweeperPos.x += Input.GetAxis(horizontalAxis) * Time.deltaTime * speed;
            sweeperPos.y += Input.GetAxis(verticalAxis) * Time.deltaTime * speed;
            transform.position = sweeperPos;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("Overlap Detected!");
        if (other.gameObject.GetComponent<Potion>() != null)
        {
            //Debug.Log("Potion Overlap Detected!");

            //if (Input.GetButtonDown(xButton))
            //{

            carriedPotions++;
            HonorsGameManager.instanceH.CleanUpScore(playerNum, 1);
            scoreText.text = carriedPotions.ToString();
            Destroy(other.gameObject);

            //}
        }
    }

    public void InitSweeper(Vector3 startingPos)
    {
        carriedPotions = 0;
        transform.position = startingPos;
    }

    public void SetControllerNumber(int PlayerNum, int ControllerNum)
    {
        controllerNum = ControllerNum;
        playerNum = PlayerNum;
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

    public int GetControllerNum()
    {
        return controllerNum;
    }

    public int GetPlayerNum()
    {
        return playerNum;
    }
}
