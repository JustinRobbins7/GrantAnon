
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

        AssignAttack();
    }

    void AssignAttack() {
        foreach (var unit in GetUnits()) {
            if (unit.attackTarget == null || !InAttackRange(unit, unit.attackTarget)) {
                unit.SetAttackTarget(FindAttackTarget(unit));
            }
        }
    }

    GameObject FindAttackTarget(Unit unit) {
        foreach (var enemyObject in GetEnemyObjects()) {
            if (InAttackRange(unit, enemyObject)) {
                return enemyObject;
            }
        }
        return null;
    }

    bool InAttackRange(Unit unit, GameObject attackTarget) {
        Node unitNode = FindObjectOfType<AStarGrid>().NodeFromWorldPoint(unit.transform.position);
        Node attackTargetNode = FindObjectOfType<AStarGrid>().NodeFromWorldPoint(attackTarget.transform.position);

        List<Node> unitNeighbors = FindObjectOfType<AStarGrid>().GetNeighbors(unitNode);

        foreach (var neighborNode in unitNeighbors) {
            if (neighborNode.Equals(attackTargetNode)) {
                return true;
            }
        }

        return false;
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
            money -= IncomeBuildingPrefab.GetComponent<IncomeBuilding>().GetCost();
        }
    }

    public void SpawnUnit(Vector2 location) {
        if (UnitRoot != null && MeleeUnitPrefab != null && money >= MeleeUnitPrefab.GetComponent<Unit>().GetCost()) {
            GameObject SpawnedUnit = Instantiate(MeleeUnitPrefab);
            SpawnedUnit.transform.parent = UnitRoot.transform;
            SpawnedUnit.transform.position = new Vector3(location.x, location.y, 0);
            SpawnedUnit.layer = SortingLayer.GetLayerValueFromName("Characters");
            SpawnedUnit.GetComponent<Unit>().SetOwningPlayerNum(PlayerNumber);
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
            SpawnedHeroUnit.GetComponent<HeroUnit>().SetOwningPlayerNum(PlayerNumber);
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
            SpawnedUnit.GetComponent<Unit>().SetOwningPlayerNum(PlayerNumber);
            money -= MeleeUnitPrefab.GetComponent<Unit>().GetCost();
        }
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
            spawnedBase.gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        }
    }

    public void SetHeroRespawn(float timeToRespawn)
    {
        heroRespawnTimer = timeToRespawn;
    }
}
