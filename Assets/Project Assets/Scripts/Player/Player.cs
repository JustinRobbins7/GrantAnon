
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


    [HideInInspector] public GameObject CentralBuildingPrefab = null;
    [HideInInspector] public GameObject IncomeBuildingPrefab = null;

    [HideInInspector] public GameObject BuildingRoot = null;
    [HideInInspector] public GameObject UnitRoot = null;
    [HideInInspector] public Vector2 baseLocation;
    [HideInInspector] public int money;
    private GameObject spawnedBase;

    private float heroRespawnTimer;

    // Start is called before the first frame update
    void Start()
    {
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
        if (BuildingRoot != null && IncomeBuildingPrefab != null) {
            GameObject SpawnedBuilding = Instantiate(IncomeBuildingPrefab);
            SpawnedBuilding.transform.parent = BuildingRoot.transform;
            SpawnedBuilding.transform.position = new Vector3(location.x, location.y, 0);
            SpawnedBuilding.layer = SortingLayer.GetLayerValueFromName("Foreground");
            SpawnedBuilding.GetComponent<IncomeBuilding>().OwningPlayerNum = PlayerNumber;
        }
    }

    public void SpawnUnit(Vector2 location) {
        if (UnitRoot != null && MeleeUnitPrefab != null) {
            GameObject SpawnedUnit = Instantiate(MeleeUnitPrefab);
            SpawnedUnit.transform.parent = UnitRoot.transform;
            SpawnedUnit.transform.position = new Vector3(location.x, location.y, 0);
            SpawnedUnit.layer = SortingLayer.GetLayerValueFromName("Characters");
            SpawnedUnit.GetComponent<Unit>().OwningPlayerNum = PlayerNumber;
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
        if (CentralBuildingPrefab != null)
        {
            spawnedBase = Instantiate(CentralBuildingPrefab);
            spawnedBase.gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        }
    }

    public void SetHeroRespawn(float timeToRespawn)
    {
        heroRespawnTimer = timeToRespawn;
    }
}
