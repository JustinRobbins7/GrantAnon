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
        
    }

    private void MoveTowardGrantOnSpawn() {
        var grant = FindObjectOfType<Grant>();
        if (grant != null) {
            foreach (var unit in player.GetUnits()) {
                unit.Move(grant.transform.position);
            }
        }
    }
}
