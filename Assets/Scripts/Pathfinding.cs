using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pathfinding : MonoBehaviour
{
    AStarGrid grid;
    PathRequestManager requestManager;

    private void Awake()
    {
        grid = GetComponent<AStarGrid>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        StartCoroutine(FindPath(startPosition, targetPosition));
    }

    private IEnumerator FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        Node startNode = grid.GetNodeFromWorldPoint(startPosition);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPosition);
        bool success = false;

        if(!targetNode.isWalkable)
        {
            Debug.LogWarning("AStarGrid: Target point for requested path is not walkable");
        }
        else
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while(openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if(currentNode == targetNode)
                {
                    success = true;
                    break;
                }

                foreach(Node neighbour in grid.GetNeighbours(currentNode).Where(n => n.isWalkable && !closedSet.Contains(n)))
                {
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                    if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if(!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        }

        yield return null;

        Vector3[] waypoints = success ? RetracePath(startNode, targetNode) : new Vector3[0];
        requestManager.FinishedProcessingPath(waypoints, success);
    }

    private Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        var waypoints = SimplifyPath(path);

        waypoints.Reverse();

        return waypoints.ToArray();
    }

    private List<Vector3> SimplifyPath(List<Node> path)
    {
        Vector2 directionOld = Vector2.zero;
        List<Vector3> waypoints = new List<Vector3>();

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX, path[i-1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
                directionOld = directionNew;
            }
        }

        return waypoints;
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return (dX > dY) ? 14 * dY + 10 * (dX - dY) : 14 * dX + 10 * (dY - dX);
    }
}
