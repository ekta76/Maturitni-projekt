using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public GameObject enemySprite;
    public GameObject attackArea;
    public float moveAfterAttacking = 1f;
    public float rotateAfterMovementAttack = 1f;
    public float checkDistance = 1f;
    public float enemyFollowSpeed = 2f;
    private Animator animator;
    private Camera mainCamera;

    private bool rotationAfterMovementAttack = false;

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }

        if (attackArea == null)
        {
            attackArea = transform.Find("AttackArea").gameObject;
        }

        if (enemySprite != null)
        {
            animator = enemySprite.GetComponent<Animator>();
        }

        mainCamera = Camera.main;
    }

    private void Update()
    {
        UpdateSpritePosition();
        UpdateSpriteRotation();

        if (!rotationAfterMovementAttack)
        {
            RotateTowardsPlayerIfClose();
        }
    }

    public void OnPlayerEnteredAttackRange()
    {
        rotationAfterMovementAttack = true;
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
        rotationAfterMovementAttack = false;
    }

    private void MoveForward()
    {
        Vector3 forwardDirection = transform.forward;
        if (Physics.Raycast(transform.position, forwardDirection, 0.5f))
        {
            Debug.Log("Object in the way. Cannot move.");
            return;
        }
        transform.position += forwardDirection * 1f;
        RoundPosition();
    }



    private void UpdateSpritePosition()
    {
        if (enemySprite != null)
        {
            Vector3 desiredPosition = transform.position;
            desiredPosition.y = -0.17f;
            enemySprite.transform.position = Vector3.MoveTowards(enemySprite.transform.position, desiredPosition, Time.deltaTime * enemyFollowSpeed);
        }
    }

    private void UpdateSpriteRotation()
    {
        if (mainCamera == null || enemySprite == null) return;

        Vector3 directionToCamera = mainCamera.transform.position - enemySprite.transform.position;
        directionToCamera.y = 0f;

        if (directionToCamera != Vector3.zero)
        {
            Quaternion lookAtCamera = Quaternion.LookRotation(-directionToCamera);
            enemySprite.transform.rotation = Quaternion.Slerp(enemySprite.transform.rotation, lookAtCamera, Time.deltaTime * 5f);
        }

        if (animator != null)
        {
            Vector3 objectToCamera = mainCamera.transform.position - enemySprite.transform.position;
            objectToCamera.y = 0f;
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
            return new Vector2(0f, 1f); // Forward
        else if (angle > 45f && angle <= 135f)
            return new Vector2(1f, 0f); // Left
        else if (angle > -135f && angle <= -45f)
            return new Vector2(-1f, 0f); // Right
        else
            return new Vector2(0f, -1f); // Back
    }

    private void RotateTowardsPlayerIfClose()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= checkDistance)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Vector3 snappedDirection = GetClosestCardinalDirection(directionToPlayer);

            Quaternion lookAtPlayer = Quaternion.LookRotation(snappedDirection);
            transform.rotation = lookAtPlayer;
        }
    }

    private Vector3 GetClosestCardinalDirection(Vector3 direction)
    {
        // Round the normalized direction to closest cardinal axis
        Vector3 cardinalDirection = Vector3.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            cardinalDirection = (direction.x > 0) ? Vector3.right : Vector3.left;
        }
        else
        {
            cardinalDirection = (direction.z > 0) ? Vector3.forward : Vector3.back;
        }

        return cardinalDirection;
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
