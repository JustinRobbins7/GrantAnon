using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIController : MonoBehaviour
{
    Player player;
    AStarGrid aStarGrid;

    // -1 means nothing selected, 0 means building selected, 1 means unit selected
    int spawnTarget = -1;

    void Awake() {
        player = GetComponent<Player>();
    }

    void Start() {
        aStarGrid = FindObjectOfType<AStarGrid>();    
    }

    void FixedUpdate() {
        SmartAttack();
        SpawnRandomItem();
    }

    private void MoveTowardGrantOnSpawn() {
        var grant = FindObjectOfType<Grant>();
        if (grant != null) {
            var units = player.units;
            foreach (var unitObject in units) {
                if (unitObject != null) {
                    var unit = unitObject.GetComponent<Unit>();
                    if (unit != null && !unit.unitAtGrant && !unit.IsMoving()) {
                        unit.Move(grant.transform.position);
                    }
                }
            }
        }
    }

    private void SpawnRandomItem() {
        if (spawnTarget == -1) {
            spawnTarget = UnityEngine.Random.Range(0, 2);
        } else if (spawnTarget == 0) {
            if (player.money > Unit.GetCost()) {
                player.SpawnUnitAtBase(true);
                spawnTarget = -1;
            }
        } else if (spawnTarget == 1) {
            if (player.money > Building.GetCost()) {
                player.SpawnBuilding(player.baseLocation);
                spawnTarget = -1;
            }
        }
    }

    private void SmartAttack() {
        if (player.units.Count > player.GetNumEnemyUnits() / 3) {
            MoveTowardGrantOnSpawn();
        }
    }
}
