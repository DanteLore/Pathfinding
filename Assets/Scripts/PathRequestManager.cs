using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PathRequestManager : MonoBehaviour
{
    private Queue<PathResult> pathResults;
    private static PathRequestManager instance;
    private Pathfinding pathfinding;

    void Awake()
    {
        instance = this;
        pathResults = new Queue<PathResult>();
        pathfinding = GetComponent<Pathfinding>();
    }

    void Update()
    {
        while(pathResults.Count > 0)
        {
            var result = pathResults.Dequeue();
            result.callback(result.path);
        }
    }

    public static void RequestPath(Vector3 startPoint, Vector3 endPoint, Action<Vector3[]> callback)
    {
        instance.DoRequestPath(startPoint, endPoint, callback);
    }

    private void DoRequestPath(Vector3 startPoint, Vector3 endPoint, Action<Vector3[]> callback)
    {
        ThreadStart threadStart = delegate 
        { 
            var path = pathfinding.FindPath(startPoint, endPoint); 

            lock(pathResults)
            {
                pathResults.Enqueue(new PathResult(path, callback));
            }
        };

        new Thread(threadStart).Start();
    }

    private struct PathResult 
    {
        public Action<Vector3[]> callback;
        public Vector3[] path;

        public PathResult(Vector3[] path, Action<Vector3[]> callback)
        {
            this.path = path;
            this.callback = callback;
        }
    }
}
