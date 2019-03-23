
using System;
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
    [HideInInspector] public Vector2 baseLocation;

    // Start is called before the first frame update
    void Start()
    {
        money = 0;
    }

    public void SetPlayerNumber(int PlayerNumber) {
        this.PlayerNumber = PlayerNumber;
    }

    public void SpawnBuilding(Vector2 location) {
        if (BuildingRoot != null && BuildingOne != null) {
            GameObject SpawnedBuilding = Instantiate(BuildingOne);
            SpawnedBuilding.transform.parent = BuildingRoot.transform;
            SpawnedBuilding.transform.position = new Vector3(location.x, location.y, 0);
            SpawnedBuilding.layer = SortingLayer.GetLayerValueFromName("Foreground");
            SpawnedBuilding.GetComponent<IncomeBuilding>().OwningPlayerNum = PlayerNumber;
        }
    }

    public void SpawnUnit(Vector2 location) {
        if (UnitRoot != null && PlayerUnit != null) {
            GameObject SpawnedUnit = Instantiate(PlayerUnit);
            SpawnedUnit.transform.parent = UnitRoot.transform;
            SpawnedUnit.transform.position = new Vector3(location.x, location.y, 0);
            SpawnedUnit.layer = SortingLayer.GetLayerValueFromName("Characters");
            SpawnedUnit.GetComponent<Unit>().OwningPlayerNum = PlayerNumber;
        }
    }

    public Unit[] GetUnits() {
        return Array.FindAll(FindObjectsOfType<Unit>(), selectableObject => selectableObject.OwningPlayerNum == PlayerNumber);
    }

    public void SetBaseLocation(Vector2 baseLocation) {
        this.baseLocation = baseLocation;
        SpawnMainBase();
        SpawnUnit(baseLocation);
    }

    public void SpawnMainBase() {
        // Method that will spawn the main base once that is ready
    }
}
