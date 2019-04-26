using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A queue used to handle requests for paths
 */
public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessingPath;

    /**
     * Intializes variables
     */
    void Awake() {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    /**
     * Method that other classes can call to get a path between two points
     */
    public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback) {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    /**
     * Processes the next pathfinding request in the queue
     */
    void TryProcessNext() {
        if (!isProcessingPath && pathRequestQueue.Count > 0) {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    /**
     * Method that's called after the pathfinding request is complete
     */
    public void FinishedProcessingPath(Vector2[] path, bool success) {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    /**
     * Struct representing a path request
     */
    struct PathRequest {
        public Vector2 pathStart;
        public Vector2 pathEnd;
        public Action<Vector2[], bool> callback;

        public PathRequest(Vector2 pathStart, Vector2 pathEnd, Action<Vector2[], bool> callback) {
            this.pathStart = pathStart;
            this.pathEnd = pathEnd;
            this.callback = callback;
        }
    }
}
