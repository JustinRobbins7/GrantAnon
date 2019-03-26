using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAIController : MonoBehaviour
{
    Player player;

    void Awake() {
        player = GetComponent<Player>();
    }

    void Update() {
        MoveTowardGrantOnSpawn();
    }

    private void MoveTowardGrantOnSpawn() {
        var grant = FindObjectOfType<Grant>();
        if (grant != null) {
            Unit[] fixedUnits = Array.FindAll(player.GetUnits(), unit => !unit.IsMoving());
            foreach (var unit in fixedUnits) {
                unit.Move(grant.transform.position);
            }
        }
    }
}
