using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public int PlayerNumber = 0;

    [SerializeField] public GameObject PlayerUnit = null;
    [SerializeField] public GameObject BuildingOne = null;

    [HideInInspector] public GameObject BuildingRoot = null;
    [HideInInspector] public GameObject UnitRoot = null;
    [HideInInspector] public int money;

    // Start is called before the first frame update
    void Start()
    {
        money = 0;
    }

    public void SetPlayerNumber(int PlayerNumber) {
        this.PlayerNumber = PlayerNumber;
    }
}
