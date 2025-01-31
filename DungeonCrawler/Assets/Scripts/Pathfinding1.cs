using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding1
{


    public static List<Node1> FindPath(Vector3 startPos, Vector3 targetPos, Grid1 gridManager)
    {
        Node1 startNode = gridManager.NodeFromWorldPoint(startPos);
        Node1 targetNode = gridManager.NodeFromWorldPoint(targetPos);

        if (!targetNode.walkable) return null;

        List<Node1> openSet = new List<Node1>();
        HashSet<Node1> closedSet = new HashSet<Node1>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node1 currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node1 neighbor in gridManager.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;

                int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
        return null;
    }

    static List<Node1> RetracePath(Node1 startNode, Node1 endNode)
    {
        List<Node1> path = new List<Node1>();
        Node1 currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    static int GetDistance(Node1 nodeA, Node1 nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstZ = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);
        return dstX + dstZ;
    }
}