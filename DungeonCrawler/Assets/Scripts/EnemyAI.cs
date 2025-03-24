using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Pathfinding))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float pathRefind = 0.05f;
    public float moveInterval = 1f;
    public float rotateToPlayerWhenClose = 1f;
    public float checkDistance = 1f;
    public LayerMask unwalkableMasks;
    public GameObject enemySprite;
    public Transform hitbox;
    public float enemyFollowSpeed = 2f;
    private Vector3 enemySpritePosition = new Vector3(0f, 0f, 0f);
    private float currentCooldown = 0f;
    public GameObject attackArea;
    public float moveAfterAttacking = 1f;
    public float rotateAfterMovementAttack = 1f;
    public bool isPlayerInAttackArea = false;
    public bool rotationAfterMovementAttack = false;
    public Grid gridManager;
    private bool canChasePlayer = false; // New flag for checking if enemy is next to player
    public float movingAnimationLengh = 1f;

    private Pathfinding pathfinding;
    private Vector3[] currentPath;
    private int targetIndex;
    private bool isMoving = false;

    private Animator animator;
    private Camera mainCamera;

    private void Start()
    {
        gridManager = FindObjectOfType<Grid>();
        pathfinding = GetComponent<Pathfinding>();
        mainCamera = Camera.main;

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found! Make sure the player has the 'Player' tag.");
        }

        // Make sure to find the attack area child if it's not set
        if (attackArea == null)
        {
            attackArea = transform.Find("AttackArea").gameObject; // Assuming your child is named "AttackArea"
        }

        // Ensure attack area has a trigger collider
        if (attackArea.GetComponent<Collider>() == null)
        {
            Debug.LogWarning("No collider found on the attack area. Make sure it has a collider set as a trigger.");
        }

        if (enemySprite != null)
        {
            animator = enemySprite.GetComponent<Animator>();
        }

        StartCoroutine(UpdatePathRoutine());
    }

    private void Update()
    {
        // Update cooldown timer
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime; // Decrease the cooldown over time
        }

        // If the cooldown is finished and not currently moving, start pathfinding or movement
        if (currentCooldown <= 0 && !isMoving && currentPath != null && targetIndex < currentPath.Length && isPlayerInAttackArea != true)
        {
            if (canChasePlayer)
            {
                StartCoroutine(MoveToNextNode());
            }
        }

        UpdateSpritePosition();
        UpdateSpriteRotation();

        if (rotationAfterMovementAttack != true)
        {
            // Rotate towards player if within checkDistance
            RotateTowardsPlayerIfClose();
        }

        // Check if the player is adjacent in all 9 directions
        CheckIfPlayerIsAdjacent();
    }


    public void OnPlayerEnteredAttackRange()
    {
        isPlayerInAttackArea = true;
        rotationAfterMovementAttack = true;
        StopCoroutine(MoveToNextNode());
        Debug.Log("Player entered attack range. Start attacking!");
    }

    public void OnPlayerExitedAttackRange()
    {
        Debug.Log("Player left attack range. Stop attacking.");
        StartCoroutine(WaitAndStartMoving());
    }

    private IEnumerator WaitAndStartMoving()
    {
        yield return new WaitForSeconds(moveAfterAttacking);
        animator.SetBool("MovingAnimation", true);
        MoveForward();
        yield return new WaitForSeconds(rotateAfterMovementAttack);
        animator.SetBool("MovingAnimation", false);
        gridManager.UpdateGridFromAnimation();
        rotationAfterMovementAttack = false;
        isPlayerInAttackArea = false;
    }

    private void MoveForward()
    {
        // Get the direction the enemy is facing (its forward direction)
        Vector3 forwardDirection = transform.forward;

        // Check if there is any obstacle (like the player) in the way within a certain distance
        float checkDistance = 1f; // You can modify this based on your game design
        if (Physics.CheckSphere(transform.position + forwardDirection * 0.5f, checkDistance, unwalkableMasks))
        {
            Debug.Log("Player is in the way. Cannot move.");
            return; // If the player is in the way, don't move
        }

        // Move the enemy 1 unit in that direction
        transform.position += forwardDirection * 1f; // 1f is the distance (you can change this if you want a different distance)

        // Optional: Round position to maintain block-based grid alignment
        RoundPosition();
    }


    private IEnumerator UpdatePathRoutine()
    {
        while (true)
        {
            Vector3 targetPosition = GetAdjustedPositionNextToPlayer();
            currentPath = pathfinding.FindPath(transform.position, targetPosition, unwalkableMasks);
            targetIndex = 0;
            yield return new WaitForSeconds(pathRefind);
        }
    }

    private IEnumerator MoveToNextNode()
    {
        isMoving = true;

        Vector3 targetPosition = currentPath[targetIndex];
        targetPosition.y = 0;

        // Rotate towards the next target position
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            yield return StartCoroutine(UpdateRotation(targetRotation, 0.25f));

            // After rotation, we wait for the cooldown before moving
            currentCooldown = moveInterval;  // Set cooldown for after rotation
            yield return new WaitForSeconds(currentCooldown);  // Wait before moving
        }

        // Check if the player is now closer than the intended movement position
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToPlayer <= checkDistance) // If player is within "alert" range
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookAtPlayer = Quaternion.LookRotation(directionToPlayer);
            yield return StartCoroutine(UpdateRotation(lookAtPlayer, 0.25f));

            // Reset cooldown and stop movement
            currentCooldown = moveInterval;
            isMoving = false;
            animator.SetBool("MovingAnimation", false);
            yield break; // Do not proceed to move
        }

        // If the player is not closer, proceed with movement
        transform.position = targetPosition;
        RoundPosition();
        targetIndex++;

        // Set cooldown for the next movement
        currentCooldown = moveInterval;
        yield return new WaitForSeconds(currentCooldown);

        isMoving = false;

        StartCoroutine(movingAnimation(movingAnimationLengh));
    }

    private IEnumerator movingAnimation(float time)
    {
        animator.SetBool("MovingAnimation", true);
        yield return new WaitForSeconds(time);
        animator.SetBool("MovingAnimation", false);
    }

    private Vector3 GetAdjustedPositionNextToPlayer()
    {
        Vector3 playerPos = player.position;
        List<Vector3> possiblePositions = new List<Vector3>
        {
            new Vector3(playerPos.x + 1, 0, playerPos.z),
            new Vector3(playerPos.x - 1, 0, playerPos.z),
            new Vector3(playerPos.x, 0, playerPos.z + 1),
            new Vector3(playerPos.x, 0, playerPos.z - 1)
        };

        List<Vector3> validPositions = new List<Vector3>();
        foreach (Vector3 pos in possiblePositions)
        {
            if (!Physics.CheckSphere(pos, 0.1f, unwalkableMasks))
            {
                validPositions.Add(pos);
            }
        }

        if (validPositions.Count == 0)
        {
            return playerPos; // Fallback if no valid positions
        }

        // Find closest valid position to enemy
        Vector3 closestPosition = validPositions[0];
        float minDistance = Vector3.Distance(transform.position, closestPosition);
        foreach (Vector3 pos in validPositions)
        {
            float currentDistance = Vector3.Distance(transform.position, pos);
            if (currentDistance < minDistance)
            {
                closestPosition = pos;
                minDistance = currentDistance;
            }
        }

        return closestPosition;
    }

    private IEnumerator UpdateRotation(Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = transform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }


    private void UpdateSpritePosition()
    {
        if (enemySprite != null)
        {
            Vector3 desiredPosition = transform.position + transform.rotation * enemySpritePosition;
            desiredPosition.y = -0.17f;  // Set the y value to -0.17
            enemySprite.transform.position = Vector3.MoveTowards(enemySprite.transform.position, desiredPosition, Time.deltaTime * enemyFollowSpeed);
        }
    }

    private void UpdateSpriteRotation()
    {
        if (mainCamera == null || enemySprite == null) return;

        // Make the sprite face the camera (billboarding effect)
        Vector3 lookAtPosition = mainCamera.transform.position;
        lookAtPosition.y = enemySprite.transform.position.y;
        enemySprite.transform.LookAt(lookAtPosition);

        // Smoothly rotate the sprite towards the target (collider's facing direction)
        Vector3 directionToPlayer = transform.forward; // The enemy's rotation direction
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        enemySprite.transform.rotation = Quaternion.Slerp(enemySprite.transform.rotation, targetRotation, Time.deltaTime * 5f); // Adjust speed as needed

        // Add 180 degrees to the Y rotation to correct the sprite's lighting issue
        enemySprite.transform.rotation *= Quaternion.Euler(0, 180, 0);

        // Update animation parameters
        if (animator != null)
        {
            Vector3 objectToCamera = mainCamera.transform.position - enemySprite.transform.position;
            objectToCamera.y = 0f; // Ignore vertical component
            objectToCamera.Normalize();

            Vector3 enemyForward = transform.forward;
            enemyForward.y = 0f;
            enemyForward.Normalize();

            float angle = Vector3.SignedAngle(enemyForward, objectToCamera, Vector3.up);
            Vector2 move = DetermineMoveDirection(angle);

            animator.SetFloat("moveX", move.x);
            animator.SetFloat("moveY", move.y);
        }
    }

    private Vector2 DetermineMoveDirection(float angle)
    {
        if (angle > -45f && angle <= 45f)
            return new Vector2(0f, 1f); // Back
        else if (angle > 45f && angle <= 135f)
            return new Vector2(1f, 0f); // Left
        else if (angle > -135f && angle <= -45f)
            return new Vector2(-1f, 0f); // Right
        else
            return new Vector2(0f, -1f); // Front
    }

    private void CheckIfPlayerIsAdjacent()
    {
        // Check if player is next to the enemy in all 9 directions
        Vector3 playerPos = player.position;
        List<Vector3> positionsToCheck = new List<Vector3>
        {
            new Vector3(playerPos.x + 0.5f, 0, playerPos.z),
            new Vector3(playerPos.x - 0.5f, 0, playerPos.z),
            new Vector3(playerPos.x, 0, playerPos.z + 0.5f),
            new Vector3(playerPos.x, 0, playerPos.z - 0.5f),
            new Vector3(playerPos.x + 0.5f, 0, playerPos.z + 0.5f),
            new Vector3(playerPos.x + 0.5f, 0, playerPos.z - 0.5f),
            new Vector3(playerPos.x - 0.5f, 0, playerPos.z + 0.5f),
            new Vector3(playerPos.x - 0.5f, 0, playerPos.z - 0.5f),
            new Vector3(playerPos.x, 0, playerPos.z)
        };

        foreach (Vector3 pos in positionsToCheck)
        {
            if (Vector3.Distance(transform.position, pos) <= checkDistance && !Physics.CheckSphere(pos, 0.1f, unwalkableMasks))
            {
                canChasePlayer = true;  // Start chasing
                break;
            }
        }
    }

    private IEnumerator RotateAndWaitBeforeMoving()
    {
        // Instant rotation of the enemy (only affects collider)
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(directionToPlayer);

        // Wait for the same duration as the move interval before continuing
        yield return new WaitForSeconds(rotateToPlayerWhenClose);

        // After rotation and waiting, resume movement or pathfinding
        isMoving = false;  // Mark as ready to move again
    }

    private void RotateTowardsPlayerIfClose()
    {
        // Check if enemy is within the defined distance from the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= checkDistance && !isMoving)
        {
            // Start the coroutine for rotating and waiting
            StartCoroutine(RotateAndWaitBeforeMoving());
            isMoving = true;  // Mark as rotating (not moving along path)
        }
    }

    private void RoundPosition()
    {
        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            0,
            Mathf.Round(transform.position.z)
        );
    }
}