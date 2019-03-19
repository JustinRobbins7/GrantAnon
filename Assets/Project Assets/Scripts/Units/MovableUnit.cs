using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableUnit : MonoBehaviour, IMoveable
{
    void Start() {
    }

    void Update() {
        if (FindObjectOfType<AStarGrid>().GridInitialized()) {
            Node node = FindObjectOfType<AStarGrid>().NodeFromWorldPoint(gameObject.GetComponent<Rigidbody2D>().position);
            Debug.Log(GetComponent<Rigidbody2D>().position);
            Debug.Log(node.worldPosition);
        }
    }

    public void Move() {

    }
}
