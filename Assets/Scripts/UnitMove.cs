using System.Collections;

using UnityEngine;

public class UnitMove : MonoBehaviour
{
    public float speed = 20f;
    private Vector3[] path;
    private int currentPathIndex = 0;
    protected Vector3 navTargetPosition = Vector3.zero;
    private bool pathRequested;

    protected void NavigateTo(Vector3 targetPos)
    {
        if(!pathRequested)
        {
            StopCoroutine("FollowPath");
            path = null;
            currentPathIndex = 0;

            navTargetPosition = targetPos;
            PathRequestManager.RequestPath(transform.position, navTargetPosition, OnPathFound);
            pathRequested = true;
        }
    }

    protected void OnPathFound(Vector3[] path)
    {
        pathRequested = false;
        if(path.Length > 0)
        {
            currentPathIndex = 0;
            this.path = path;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    protected IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while(true)
        {
            if(transform.position == currentWaypoint)
            {
                currentPathIndex++;
                if(currentPathIndex >= path.Length)
                    yield break;

                currentWaypoint = path[currentPathIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

            yield return null;
        }
    }

    protected void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = currentPathIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);
                if(i == currentPathIndex)
                    Gizmos.DrawLine(transform.position, path[i]);
                else
                    Gizmos.DrawLine(path[i-1], path[i]);
            }
        }
    }
}
