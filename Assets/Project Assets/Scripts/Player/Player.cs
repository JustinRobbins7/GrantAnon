
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public int PlayerNumber = 0;

    [HideInInspector] public GameObject HeroUnitPrefab = null;
    [HideInInspector] public GameObject SpawnedHeroUnit = null;
    [HideInInspector] public GameObject MeleeUnitPrefab = null;
    List<GameObject> units = null;

    [HideInInspector] public GameObject CentralBuildingPrefab = null;
    [HideInInspector] public GameObject IncomeBuildingPrefab = null;

    [SerializeField] public GameObject BuildingOne = null;
    [HideInInspector] public GameObject BuildingRoot = null;

    List<GameObject> incomeBuildings = null;
    [HideInInspector] public GameObject UnitRoot = null;
    [HideInInspector] public Vector2 baseLocation;
    [HideInInspector] public int money;
    private GameObject spawnedBase;

    private float heroRespawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        units = new List<GameObject>();
        incomeBuildings = new List<GameObject>();

        money = 0;
        heroRespawnTimer = 0;
    }

    void FixedUpdate()
    {
        if (heroRespawnTimer >= 0)
        {
            heroRespawnTimer -= Time.deltaTime;

            if (heroRespawnTimer < 0)
            {
                heroRespawnTimer = 0;
                SpawnHeroUnit(baseLocation);
            }
        }
    }

    public void SetPlayerNumber(int PlayerNumber) {
        this.PlayerNumber = PlayerNumber;
    }

    public void SpawnBuilding(Vector2 location) {
        if (BuildingRoot != null && IncomeBuildingPrefab != null && money >= IncomeBuildingPrefab.GetComponent<IncomeBuilding>().GetCost()) {
            GameObject SpawnedBuilding = Instantiate(IncomeBuildingPrefab);
            SpawnedBuilding.transform.parent = BuildingRoot.transform;
            SpawnedBuilding.transform.position = new Vector3(location.x, location.y, 0);
            SpawnedBuilding.layer = SortingLayer.GetLayerValueFromName("Foreground");
            SpawnedBuilding.GetComponent<IncomeBuilding>().OwningPlayerNum = PlayerNumber;
            money -= IncomeBuildingPrefab.GetComponent<IncomeBuilding>().GetCost();
        }
    }

    public void SpawnUnit(Vector2 location) {
        if (UnitRoot != null && MeleeUnitPrefab != null && money >= MeleeUnitPrefab.GetComponent<Unit>().GetCost()) {
            GameObject SpawnedUnit = Instantiate(MeleeUnitPrefab);
            SpawnedUnit.transform.parent = UnitRoot.transform;
            SpawnedUnit.transform.position = new Vector3(location.x, location.y, 0);
            SpawnedUnit.layer = SortingLayer.GetLayerValueFromName("Characters");
            SpawnedUnit.GetComponent<Unit>().OwningPlayerNum = PlayerNumber;
            money -= MeleeUnitPrefab.GetComponent<Unit>().GetCost();
        }
    }

    public void SpawnHeroUnit(Vector2 location)
    {
        if (SpawnedHeroUnit == null)
        {
            SpawnedHeroUnit = Instantiate(HeroUnitPrefab);
            SpawnedHeroUnit.transform.parent = UnitRoot.transform;
            SpawnedHeroUnit.transform.position = new Vector3(location.x, location.y, 0);
            SpawnedHeroUnit.name = "Player " + PlayerNumber.ToString() + " Hero";
            SpawnedHeroUnit.GetComponent<HeroUnit>().owner = this;
            SpawnedHeroUnit.GetComponent<HeroUnit>().OwningPlayerNum = PlayerNumber;
        }
    }

    public void SpawnUnitAtBase()
    {
        if (UnitRoot != null && MeleeUnitPrefab != null && money >= MeleeUnitPrefab.GetComponent<Unit>().GetCost())
        {
            GameObject SpawnedUnit = Instantiate(MeleeUnitPrefab);
            SpawnedUnit.transform.parent = UnitRoot.transform;
            SpawnedUnit.transform.position = new Vector3(baseLocation.x, baseLocation.y, 0);
            SpawnedUnit.layer = SortingLayer.GetLayerValueFromName("Characters");
            SpawnedUnit.GetComponent<Unit>().OwningPlayerNum = PlayerNumber;
            money -= MeleeUnitPrefab.GetComponent<Unit>().GetCost();
        }
    }

    public Unit[] GetUnits() {
        return Array.FindAll(FindObjectsOfType<Unit>(), selectableObject => selectableObject.OwningPlayerNum == PlayerNumber);
    }

    public void SetBaseLocation(Vector2 baseLocation) {
        this.baseLocation = baseLocation;
        SpawnMainBase();
        //SpawnUnit(baseLocation);
        SpawnHeroUnit(baseLocation);
    }

    public void SpawnMainBase() {
        // Method that will spawn the main base once that is ready
        spawnedBase = Instantiate(CentralBuildingPrefab);
        spawnedBase.transform.position = baseLocation;
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

        spawnedBase.SetActive(false);
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

        spawnedBase.SetActive(true);
    }

    public void SetHeroRespawn(float timeToRespawn)
    {
        heroRespawnTimer = timeToRespawn;
    }
}
