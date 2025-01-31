using UnityEngine;

public class Node
{
    public bool walkable; // Can the node be walked on?
    public Vector3 worldPosition; // Position in the world
    public int gridX, gridY; // Node's position in the grid

    public int gCost; // Cost from start node
    public int hCost; // Heuristic cost to end node
    public Node parent; // Parent node for path retracing

    public int fCost => gCost + hCost; // Total cost (fCost)

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }
}