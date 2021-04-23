using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    private float nodeDiameter;
    private Node[,] grid;

    private int gridSizeX, gridSizeY;

    public int MaxSize{
        get{ return gridSizeX * gridSizeY; }
    }

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    private void CreateGrid()
    {
        if(gridWorldSize.x <= 0 || gridWorldSize.y <= 0)
            Debug.LogError("AStarGrid: You must set GridWorldSize to be larger than 0 on both axes");

        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                grid[x,y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public IEnumerable<Node> GetNeighbours(Node node)
    {
        for(int x = node.gridX - 1; x <= node.gridX + 1; x++)
        {
            for(int y = node.gridY - 1; y <= node.gridY + 1; y++)
            {
                if(x == node.gridX && y == node.gridY)
                    continue;

                if(x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
                    yield return grid[x, y];
            }
        }
    }

    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }
}
