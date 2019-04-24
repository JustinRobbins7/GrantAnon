
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public int PlayerNumber = 0;

    [HideInInspector] public GameObject HeroUnitPrefab = null;
    [HideInInspector] public GameObject SpawnedHeroUnit = null;
    [HideInInspector] public GameObject MeleeUnitPrefab = null;
    [HideInInspector] public List<GameObject> units;

    [HideInInspector] public GameObject CentralBuildingPrefab = null;
    [HideInInspector] public GameObject IncomeBuildingPrefab = null;
    [HideInInspector] public List<GameObject> incomeBuildings;

    [HideInInspector] public GameObject BuildingRoot = null;
    [HideInInspector] public GameObject UnitRoot = null;
    [HideInInspector] public Vector2 baseLocation;
    [HideInInspector] public int money;
    [HideInInspector] public int grant;
    private GameObject spawnedBase;

    private float heroRespawnTimer;
    private List<GameObject> enemyUnits;

    //AStarGrid grid = null;

    // Start is called before the first frame update
    void Start()
    {
        money = 0;
        grant = 0;
        heroRespawnTimer = 0;

        if (units == null)
        {
            units = new List<GameObject>();
        }

        if (incomeBuildings == null)
        {
            incomeBuildings = new List<GameObject>();
        }

        if (enemyUnits == null)
        {
            enemyUnits = new List<GameObject>();
        }
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
            SpawnedBuilding.GetComponent<IncomeBuilding>().SetOwningPlayerNum(PlayerNumber);
            incomeBuildings.Add(SpawnedBuilding);
            FindObjectOfType<UnitController>().AddDamageable(SpawnedBuilding);
            money -= IncomeBuildingPrefab.GetComponent<IncomeBuilding>().GetCost();
        }
    }

    public void SpawnUnit(Vector2 location) {
        if (UnitRoot != null && MeleeUnitPrefab != null && money >= MeleeUnitPrefab.GetComponent<Unit>().GetCost()) {
            Debug.Log("Pre Spawn Unit Count: " + units.Count.ToString());
            GameObject SpawnedUnit = Instantiate(MeleeUnitPrefab);
            SpawnedUnit.transform.parent = UnitRoot.transform;
            SpawnedUnit.transform.position = new Vector3(location.x, location.y, 0);
            SpawnedUnit.layer = SortingLayer.GetLayerValueFromName("Characters");
            SpawnedHeroUnit.GetComponent<Unit>().owner = this;
            SpawnedUnit.GetComponent<Unit>().SetOwningPlayerNum(PlayerNumber);
            units.Add(SpawnedUnit);
            FindObjectOfType<UnitController>().AddDamageable(SpawnedUnit);
            Debug.Log("Current Unit Count: " + units.Count.ToString());
            money -= MeleeUnitPrefab.GetComponent<Unit>().GetCost();
        }
        else
        {
            if (UnitRoot == null)
            {
                Debug.Log("UnitRoot is null!");
            }

            if (MeleeUnitPrefab == null)
            {
                Debug.Log("MeleeUnitPrefab is null!");
            }

            if (money >= MeleeUnitPrefab.GetComponent<Unit>().GetCost())
            {
                Debug.Log("Not enough money!");
            }
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
            SpawnedHeroUnit.GetComponent<HeroUnit>().SetOwningPlayerNum(PlayerNumber);
            FindObjectOfType<UnitController>().AddDamageable(SpawnedHeroUnit);
            units.Add(SpawnedHeroUnit);
            Debug.Log("Current Unit Count: " + units.Count.ToString());
        }
    }

    public void SpawnUnitAtBase()
    {
        SpawnUnit(baseLocation);
    }

    public Unit[] GetUnits() {
        return Array.FindAll(FindObjectsOfType<Unit>(), selectableObject => selectableObject.GetOwningPlayerNum() == PlayerNumber);
    }

    public GameObject[] GetEnemyObjects() {
        
        GameObject[] units = Array.FindAll(FindObjectsOfType<Unit>(), selectableObject => selectableObject.GetOwningPlayerNum() != PlayerNumber).Select(unit => unit.gameObject).ToArray();
        GameObject[] buildings = Array.FindAll(FindObjectsOfType<IncomeBuilding>(), building => building.GetOwningPlayerNum() != PlayerNumber).Select(incomeBuilding => incomeBuilding.gameObject).ToArray();

        GameObject[] gameObjects = new GameObject[units.Length + buildings.Length];
        Array.Copy(units, gameObjects, units.Length);
        Array.Copy(buildings, 0, gameObjects, units.Length, buildings.Length);
        

        return gameObjects;
    }

    public void SetBaseLocation(Vector2 baseLocation) {
        this.baseLocation = baseLocation;
        SpawnMainBase();
        SpawnHeroUnit(baseLocation);
    }

    public void SpawnMainBase() {
        // Method that will spawn the main base once that is ready
        if (CentralBuildingPrefab != null)
        {
            spawnedBase = Instantiate(CentralBuildingPrefab);
            spawnedBase.gameObject.GetComponent<CentralBuilding>().SetOwningPlayerNum(PlayerNumber);
            spawnedBase.gameObject.transform.position = new Vector2(baseLocation.x, baseLocation.y);
        }
    }

    public void SetHeroRespawn(float timeToRespawn)
    {
        heroRespawnTimer = timeToRespawn;
    }
}
