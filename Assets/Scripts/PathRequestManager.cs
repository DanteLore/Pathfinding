using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequests;
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    Pathfinding pathfinding;
    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        pathRequests = new Queue<PathRequest>();
        pathfinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 startPoint, Vector3 endPoint, Action<Vector3[], bool> callback)
    {
        PathRequest request = new PathRequest(startPoint, endPoint, callback);
        instance.pathRequests.Enqueue(request);
        instance.TryProcessNext();
    }

    private void TryProcessNext()
    {
        if(!isProcessingPath && pathRequests.Count > 0)
        {
            currentPathRequest = pathRequests.Dequeue();
            isProcessingPath = true;
            pathfinding.StartFindPath(currentPathRequest.startPoint, currentPathRequest.endPoint);
        }
    }

    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest{
        public Vector3 startPoint;
        public Vector3 endPoint;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 startPoint, Vector3 endPoint, Action<Vector3[], bool> callback)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
            this.callback = callback;
        }
    }
}
