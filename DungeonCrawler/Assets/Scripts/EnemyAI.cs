using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Pathfinding))]
public class EnemyAI : MonoBehaviour
{
    public Transform player; // The target (player)
    public float pathRefind = 0.05f; // Time between path recalculations
    public float moveInterval = 2f; // Cooldown before taking the next step
    public float checkDistance = 1.1f; // Slightly more than 1 to detect obstacles
    public LayerMask unwalkableMasks; // Add unwalkable layers/masks (player, walls, etc.)

    private Pathfinding pathfinding; // Reference to Pathfinding script
    private Vector3[] currentPath; // The calculated path
    private int targetIndex; // Current position in the path
    private bool isMoving = false; // Is the enemy currently moving

    private void Start()
    {
        pathfinding = GetComponent<Pathfinding>();
        StartCoroutine(UpdatePathRoutine());
    }

    private void Update()
    {
        // Enemy only moves if not already moving
        if (!isMoving && currentPath != null && targetIndex < currentPath.Length)
        {
            StartCoroutine(MoveToNextNode());
        }
    }

    private IEnumerator UpdatePathRoutine()
    {
        while (true)
        {
            // Calculate the position directly in front of the player
            Vector3 targetPosition = GetAdjustedPositionInFrontOfPlayer();

            // Find the path to the target considering the unwalkable layers
            currentPath = pathfinding.FindPath(transform.position, targetPosition, unwalkableMasks);

            targetIndex = 0; // Reset path index
            yield return new WaitForSeconds(pathRefind); // Wait before recalculating the path
        }
    }

    private IEnumerator MoveToNextNode()
    {
        isMoving = true;

        // Get the next target position on the path
        Vector3 targetPosition = currentPath[targetIndex];
        targetPosition.y = 0; // Ensure the enemy stays at height 1

        // Move instantly to the target position
        transform.position = targetPosition;

        // Snap position to grid for accuracy
        RoundPosition();

        // Move to the next node in the path
        targetIndex++;

        // Wait for the cooldown before moving to the next node
        yield return new WaitForSeconds(moveInterval);

        isMoving = false;
    }

    private Vector3 GetAdjustedPositionInFrontOfPlayer()
    {
        // Get the player's facing direction
        Vector3 forward = player.forward;

        // Snap the direction to the nearest axis (no diagonal movement)
        forward = new Vector3(
            Mathf.Round(forward.x),
            0, // Ensure no Y-axis movement
            Mathf.Round(forward.z)
        );

        // Adjust to stand one block closer in front of the player
        Vector3 targetPosition = player.position + forward;

        // Round X and Z to the nearest 1, and ensure Y is at height 1
        return new Vector3(
            Mathf.Round(targetPosition.x),
            0, // Set height explicitly
            Mathf.Round(targetPosition.z)
        );
    }

    private void RoundPosition()
    {
        // Round the enemy's position to ensure alignment with the grid
        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            0, // Keep the enemy at height 1
            Mathf.Round(transform.position.z)
        );
    }
}
