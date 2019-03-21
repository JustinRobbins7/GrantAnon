using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IMoveable, ISelectable
{
    public int OwningControllerNum = 0;

    Vector2[] path;
    [SerializeField] float movementSpeed = 5f;
    int targetIndex;

    private bool selected = false;
    private float radius = .5f;

    void Update() {
        // UpdateMovement();
        if (selected && targetIndex > 0) {
            gameObject.UpdateCircleDraw(radius);
        }
    }

    public void Move(Vector2 target) {
        PathRequestManager.RequestPath(GetComponent<Rigidbody2D>().position, target, OnPathFound);
        // path = FindObjectOfType<Pathfinding>().FindPath(GetComponent<Rigidbody2D>().position, target);
    }

    /* private void UpdateMovement() {
        if (path != null && path.Count > 0) {
            gameObject.UpdateCircleDraw(radius);

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
    }*/

    private void OnPathFound(Vector2[] path, bool success) {
        if (success) {
            StopCoroutine("FollowPath");
            this.path = path;
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {
        Vector2 currentWaypoint = path[0];
        targetIndex = 0;

        while (true) {
            if (GetComponent<Rigidbody2D>().position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    targetIndex = 0;
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            GetComponent<Rigidbody2D>().position = Vector2.MoveTowards(GetComponent<Rigidbody2D>().position, currentWaypoint, movementSpeed * Time.deltaTime);
            gameObject.UpdateCircleDraw(radius);
            yield return null;
        }
    }

    public void SetSelected(bool selected) {
        // If no change then don't update
        if (this.selected == selected) {
            return;
        }

        if (this.selected) {
            gameObject.DestroyCircleDraw();

            this.selected = false;

        } else {
            gameObject.CreateCircleDraw(radius);

            this.selected = true;
        }
    }

    public bool IsSelected() {
        return selected;
    }
}
