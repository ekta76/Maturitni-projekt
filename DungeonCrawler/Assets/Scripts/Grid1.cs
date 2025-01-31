using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Grid1 : MonoBehaviour
{
    public Vector2 gridWorldSize;
    public LayerMask unwalkableMask;

    Node1[,] grid;
    float nodeDiameter = 1f;
    int gridSizeX, gridSizeZ;

    public List<Node1> path;


    void Awake()
    {
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.y);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node1[gridSizeX, gridSizeZ];
        Vector3 worldBottomLeft = transform.position -
            Vector3.right * gridWorldSize.x / 2 -
            Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 worldPoint = worldBottomLeft +
                    Vector3.right * (x * nodeDiameter) +
                    Vector3.forward * (z * nodeDiameter);

                // Check for obstacles at gameplay height (Y=0)
                Vector3 obstacleCheckPos = new Vector3(
                    worldPoint.x,
                    transform.position.y, // Y=0 level
                    worldPoint.z
                );

                bool walkable = !Physics.CheckSphere(
                    obstacleCheckPos,
                    nodeDiameter / 2,
                    unwalkableMask
                );

                grid[x, z] = new Node1(walkable, obstacleCheckPos, x, z);
            }
        }
    }

    public Node1 NodeFromWorldPoint(Vector3 worldPosition)
    {
        Vector3 localPos = worldPosition - transform.position;
        int x = Mathf.RoundToInt((localPos.x + gridWorldSize.x / 2 - nodeDiameter / 2) / nodeDiameter);
        int z = Mathf.RoundToInt((localPos.z + gridWorldSize.y / 2 - nodeDiameter / 2) / nodeDiameter);
        x = Mathf.Clamp(x, 0, gridSizeX - 1);
        z = Mathf.Clamp(z, 0, gridSizeZ - 1);
        return grid[x, z];
    }

    public List<Node1> GetNeighbors(Node1 node)
    {
        List<Node1> neighbors = new List<Node1>();

        // Horizontal and vertical directions only
        int[] dx = { -1, 1, 0, 0 };
        int[] dz = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int checkX = node.gridX + dx[i];
            int checkZ = node.gridZ + dz[i];

            if (checkX >= 0 && checkX < gridSizeX && checkZ >= 0 && checkZ < gridSizeZ)
            {
                neighbors.Add(grid[checkX, checkZ]);
            }
        }
        return neighbors;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node1 node in grid)
            {
                Gizmos.color = node.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }

            // Visualize the path (if available)
            if (path != null)
            {
                Gizmos.color = Color.black;
                for (int i = 0; i < path.Count - 1; i++)
                {
                    // Draw a black line between each consecutive node in the path
                    Gizmos.DrawLine(path[i].worldPosition, path[i + 1].worldPosition);
                }
            }
        }
    }
}

public class Node1
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridZ;
    public int gCost;
    public int hCost;
    public Node1 parent;

    public Node1(bool _walkable, Vector3 _worldPos, int _gridX, int _gridZ)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridZ = _gridZ;
    }

    public int fCost => gCost + hCost;
}