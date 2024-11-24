/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool smoothMovement = true;
    public float transitionMovement = 5f;
    public float transitionSpeed = 500f;

    private Vector3 targetGridPos;
    private Vector3 prevTargetPos;
    private Vector3 targetRotation;
    private bool collidedWithWall = false; // Flag to track collision with walls

    private void Start()
    {
        targetGridPos = Vector3Int.RoundToInt(transform.position);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (collidedWithWall)
        {
            // If collided with a wall, reset the flag and skip moving this frame
            collidedWithWall = false;
            targetGridPos = prevTargetPos; // Reset target position to avoid moving into the wall
        }
        else
        {
            prevTargetPos = transform.position; // Update previous position
        }

        Vector3 targetPosition = targetGridPos;

        if (targetRotation.y > 270f && targetRotation.y < 361f) targetRotation.y = 0f;
        if (targetRotation.y < 0f) targetRotation.y = 270f;

        if (smoothMovement)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * transitionMovement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * transitionSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the tag "Wall"
        if (collision.gameObject.CompareTag("Wall"))
        {
            collidedWithWall = true; // Set the flag to true
        }
    }

    bool AtRest
    {
        get
        {
            return Vector3.Distance(transform.position, targetGridPos) < 0.05f && Vector3.Distance(transform.eulerAngles, targetRotation) < 0.05f;
        }
    }

    // Movement and rotation methods remain unchanged
    public void RotateLeft() { if (AtRest) targetRotation -= Vector3.up * 90f; }
    public void RotateRight() { if (AtRest) targetRotation += Vector3.up * 90f; }
    public void MoveForward() { if (AtRest) targetGridPos += transform.forward; }
    public void MoveBackwards() { if (AtRest) targetGridPos -= transform.forward; }
    public void MoveLeft() { if (AtRest) targetGridPos -= transform.right; }
    public void MoveRight() { if (AtRest) targetGridPos += transform.right; }

    /*
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided");

        if (collision.gameObject.tag == "Wall")
        {
            transform.position = prevTargetPos;
        }
    }
    
}
*/
using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float checkDistance = 1.1f; // Slightly more than 1 to detect walls effectively
    public float moveSpeed = 5f; // Speed of movement
    public float rotationSpeed = 360f; // Degrees per second, adjust for rotation speed
    private bool isMoving = false;

    void Update()
    {
        if (!isMoving)
        {
            HandleMovement();
            HandleRotation();
        }
    }

    private void HandleMovement()
    {
        Vector3 direction = Vector3.zero;
        Vector3 targetPosition = transform.position;

        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = transform.forward;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = -transform.forward;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = -transform.right;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = transform.right;
        }

        if (direction != Vector3.zero && CanMove(direction))
        {
            targetPosition += direction; // Move one tile in the desired direction
            StartCoroutine(MoveToPosition(targetPosition));
        }
    }

    private void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(SmoothRotate(-90)); // Rotate left
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(SmoothRotate(90)); // Rotate right
        }
    }

    private bool CanMove(Vector3 direction)
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + new Vector3(0, 0.5f, 0); // Adjust as needed

        if (Physics.Raycast(rayOrigin, direction, out hit, checkDistance))
        {
            if (hit.collider.CompareTag("Wall"))
            {
                return false; // Can't move in this direction
            }
        }
        return true; // Can move
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }
        RoundPosition();
        isMoving = false;
    }

    private IEnumerator SmoothRotate(float angle)
    {
        isMoving = true;
        var originalRotation = transform.rotation;
        var targetAngle = transform.eulerAngles + Vector3.up * angle;
        var targetRotation = Quaternion.Euler(RoundVector(targetAngle));
        var step = 0f; // Progress from 0 to 1

        while (step < 1f)
        {
            step += Time.deltaTime * rotationSpeed / 90f; // Adjust step increment based on rotation speed and desired angle
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, step);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(RoundVector(targetRotation.eulerAngles)); // Ensure the rotation is exactly the target rotation
        isMoving = false;
    }

    private void RoundPosition()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y, // Assuming Y doesn't need to be rounded in this context
            transform.position.z);
    }

    private Vector3 RoundVector(Vector3 vector)
    {
        return new Vector3(
            Mathf.Round(vector.x),
            Mathf.Round(vector.y),
            Mathf.Round(vector.z));
    }
}












