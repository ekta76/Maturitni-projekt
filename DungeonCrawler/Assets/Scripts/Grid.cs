using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask; // Layer for obstacles
    public Vector2 gridWorldSize; // Size of the grid in world units (X and Z axes)
    public float nodeRadius; // Radius of each node
    public Transform player; // Reference to the player
    public float updateInterval = 0.05f; // Time interval for grid updates
    private Vector3 lastPlayerPosition;

    Node[,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeY;

    public List<Node> path; // Store the current path

    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }

    private void Update()
    {
            // Check if player moved since last frame
            if (Vector3.Distance(player.position, lastPlayerPosition) > 0.01f)
            {
                CreateGrid(); // Update grid only when player moves
                lastPlayerPosition = player.position; // Save new position
            }
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask);
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public void UpdateGridFromAnimation()
    {
        CreateGrid();
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        // Adjust world position relative to the grid's position
        Vector3 localPos = worldPosition - transform.position;
        float percentX = (localPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (localPos.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        // Only horizontal and vertical neighbors
        int[,] directions = new int[,]
        {
            { 0, 1 },  // Up
            { 0, -1 }, // Down
            { 1, 0 },  // Right
            { -1, 0 }  // Left
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int checkX = node.gridX + directions[i, 0];
            int checkY = node.gridY + directions[i, 1];

            if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
            {
                neighbours.Add(grid[checkX, checkY]);
            }
        }

        return neighbours;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null)
        {
            foreach (Node node in grid)
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
