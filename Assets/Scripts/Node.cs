using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : IHeapItem<Node>
{
    public bool isWalkable;
    public Vector3 worldPosition;

    public int index {get; set;}

    public int gCost;
    public int hCost;

    public int gridX;
    public int gridY;

    public int fCost 
    {
        get { return gCost + hCost; }
    }

    public Node parent;

    public Node(bool isWalkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.isWalkable = isWalkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public int CompareTo(Node other)
    {
        int compare = this.fCost.CompareTo(other.fCost);

        return compare != 0 ? compare : this.hCost.CompareTo(other.hCost);
    }

    public override string ToString()
    {
        return "Node x=" + gridX + " y=" + gridY + " gCost=" + gCost + "hCost=" + hCost + " fCost=" + fCost;
    }

    public static bool operator <(Node n1, Node n2) 
    {
        return n1.CompareTo(n2) < 0;
    }

    public static bool operator >(Node n1, Node n2) 
    {
        return n1.CompareTo(n2) > 0;
    }
}
