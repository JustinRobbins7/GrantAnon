using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableUnit : MonoBehaviour, IMoveable
{
    private List<Node> path = new List<Node>();
    [SerializeField] float movementSpeed = 50f;

    void Update() {
        UpdateMovement();
    }

    public void Move(Vector2 target) {
        path = FindObjectOfType<Pathfinding>().FindPath(GetComponent<Rigidbody2D>().position, target);
    }

    private void UpdateMovement() {
        if (path != null && path.Count > 0) {
            Vector2 targetPos = FindObjectOfType<AStarGrid>().NodeToWorldPosition(path[0]);
            Vector2 currPos = transform.position;

            if (Math.Round(targetPos.x, 1) == Math.Round(currPos.x, 1) && Math.Round(targetPos.y, 1) == Math.Round(currPos.y, 1)) {
                path.RemoveAt(0);
                if (path.Count > 0) {
                    targetPos = FindObjectOfType<AStarGrid>().NodeToWorldPosition(path[0]);
                } else {
                    return;
                }
            }

            Vector2 direction = (targetPos - currPos).normalized;
            GetComponent<Rigidbody2D>().MovePosition(currPos + direction * movementSpeed * Time.deltaTime);
        }
    }
}
