
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Player class that maintains the units and buildings of each player,
 * including prefabs of the units they create and lists of these units.
 * Also maintains the money and grants gained by the player.
 * Also respawns their hero units when necessary.
 */
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

    [SerializeField] public GameObject BuildingOne = null;
    [HideInInspector] public GameObject BuildingRoot = null;

    [HideInInspector] public GameObject UnitRoot = null;
    [HideInInspector] public Vector2 baseLocation;
    [HideInInspector] public int money;
    [HideInInspector] public int grant;
    private GameObject spawnedBase;

    private float heroRespawnTimer;
    private List<GameObject> enemyUnits;

    //AStarGrid grid = null;

    /**
     * The Start method initializes the values and lists of the Player class
     */
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

    /**
     * If the hero is dead, counts down the timer until he needs to be spawned, then calls SpawnHeroUnit.
     */
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

    /**
     * Sets the player number associated with this instance of Player.
     */
    public void SetPlayerNumber(int PlayerNumber) {
        this.PlayerNumber = PlayerNumber;
    }

    /**
     * Spawns an income building at the specificed location, adds it to the income building list, then decrements its cost from the player's money.
     */
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

    /**
     * Spawns an unit at the specified location, adds it to the unit list, then decrements its cost from the player's money.
     */
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

    /**
     * Spawns a hero unit at the specified location and adds it to the unit list.
     */
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

    /**
     * Spawns a unit at this player's central building
     */
    public void SpawnUnitAtBase()
    {
        SpawnUnit(baseLocation);
    }

    /**
     * Searches the scene for every unit owned by this player and return them in an array
     */
    public Unit[] GetUnits() {
        return Array.FindAll(FindObjectsOfType<Unit>(), selectableObject => selectableObject.GetOwningPlayerNum() == PlayerNumber);
    }

    /**
     * Searches the scene for every unit and building not owned by this player and return them in an array
     */
    public GameObject[] GetEnemyObjects() {
        
        GameObject[] units = Array.FindAll(FindObjectsOfType<Unit>(), selectableObject => selectableObject.GetOwningPlayerNum() != PlayerNumber).Select(unit => unit.gameObject).ToArray();
        GameObject[] buildings = Array.FindAll(FindObjectsOfType<IncomeBuilding>(), building => building.GetOwningPlayerNum() != PlayerNumber).Select(incomeBuilding => incomeBuilding.gameObject).ToArray();

        GameObject[] gameObjects = new GameObject[units.Length + buildings.Length];
        Array.Copy(units, gameObjects, units.Length);
        Array.Copy(buildings, 0, gameObjects, units.Length, buildings.Length);
        

        return gameObjects;
    }

    /**
     * Sets the location for the central building, then spawns it and a hero unit.
     */
    public void SetBaseLocation(Vector2 baseLocation) {
        this.baseLocation = baseLocation;
        SpawnMainBase();
        SpawnHeroUnit(baseLocation);
    }

    /**
     * Spawns the central building at the location specified by the baseLocation variable
     */
    public void SpawnMainBase() {
        // Method that will spawn the main base once that is ready
        //spawnedBase = Instantiate(CentralBuildingPrefab);
        //spawnedBase.transform.position = baseLocation;

        spawnedBase = Instantiate(CentralBuildingPrefab);
        spawnedBase.gameObject.GetComponent<CentralBuilding>().SetOwningPlayerNum(PlayerNumber);
        spawnedBase.gameObject.transform.position = new Vector2(baseLocation.x, baseLocation.y);
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

    /**
     * Resets the timer that respawns this player's hero
     */
    public void SetHeroRespawn(float timeToRespawn)
    {
        heroRespawnTimer = timeToRespawn;
    }
}
