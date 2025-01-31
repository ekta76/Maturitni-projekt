using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI1 : MonoBehaviour
{
    public Transform player;
    public Grid1 gridManager;
    public LayerMask layerMask;
    public Transform visualChild;
    public float rotationSpeed = 90f;
    public float moveInterval = 2f;

    private bool isRotating;
    private bool isMoving;
    private Coroutine activeCoroutine;
    private Vector3 previousPosition;

    void Start()
    {
        StartCoroutine(MovementCycle());
    }

    IEnumerator MovementCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveInterval);
            if (IsAdjacentToPlayer()) continue;

            List<Node1> path = Pathfinding1.FindPath(transform.position, player.position, gridManager);
            if (path != null && path.Count > 1)
            {
                Vector3 moveDirection = (path[1].worldPosition - transform.position).normalized;

                // Rotation phase
                isRotating = true;
                //yield return StartCoroutine(RotateOverTime(moveDirection, 1f));
                isRotating = false;

                // Cooldown after rotation
                yield return new WaitForSeconds(1f);

                // Movement execution
                previousPosition = transform.position;
                transform.position = path[1].worldPosition;
                StartCoroutine(SmoothMoveAnimation(previousPosition, transform.position, 0.5f));
            }
        }
    }

    IEnumerator RotateOverTime(Vector3 direction, float duration)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                elapsed / duration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    IEnumerator SmoothMoveAnimation(Vector3 start, Vector3 end, float duration)
    {
        visualChild.localPosition = start - end;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            visualChild.localPosition = Vector3.Lerp(
                start - end,
                Vector3.zero,
                elapsed / duration
            );
            elapsed += Time.deltaTime;
            yield return null;
        }
        visualChild.localPosition = Vector3.zero;
    }

    void Update()
    {
        if (!isRotating && !isMoving && IsAdjacentToPlayer())
        {
            if (activeCoroutine == null)
            {
                activeCoroutine = StartCoroutine(DelayedPlayerRotation());
            }
        }
    }

    IEnumerator DelayedPlayerRotation()
    {
        yield return new WaitForSeconds(1f);
        Vector3 direction = (player.position - transform.position).normalized;
        yield return StartCoroutine(RotateOverTime(direction, 1f));
        activeCoroutine = null;
    }

    bool IsAdjacentToPlayer()
    {
        Vector3 enemyPos = transform.position;
        Vector3 playerPos = player.position;
        return Mathf.Abs(enemyPos.x - playerPos.x) + Mathf.Abs(enemyPos.z - playerPos.z) <= 1f;
    }
}