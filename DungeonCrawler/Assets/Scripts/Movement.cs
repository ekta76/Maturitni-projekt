using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    public float checkDistance = 1.1f; // Slightly more than 1 to detect walls effectively
    public float rotationSpeed = 360f; // Degrees per second, adjust for rotation speed
    public float cameraRotationSpeed = 5f; // Speed at which the camera rotates
    public float movementCooldown = 0.3f; // Cooldown time for movement
    private bool canMove = true;
    private bool isMoving = false;
    public Camera playerCamera;
    public float cameraFollowSpeed = 4f; // Speed at which the camera follows the player
    private Vector3 cameraOffset = new Vector3(0, -0.2f, -0.25f);
    private Vector3 lastSafePosition;
    public GameObject eventSystem;

    public string[] clickableTags = { "FrontChain", "BackChain", "Button", "Lever" };
    private List<Collider> clickableColliders = new List<Collider>();

    AudioManager audioManager;

    void Start()
    {
        lastSafePosition = transform.position;
        FindClickableObjects();
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void FindClickableObjects()
    {
        foreach (string tag in clickableTags)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objects)
            {
                Collider col = obj.GetComponent<Collider>();
                if (col != null)
                {
                    clickableColliders.Add(col);
                }
            }
        }
    }

    void Update()
    {
        if (canMove)
        {
            HandleMovement();
        }
        if (!isMoving)
        {
            HandleRotation();
        }
        UpdateCameraPosition();
        RoundPlayerPosition();
        CheckIfStuck();

        if (isMoving)
        {
            SetClickableColliders(false);
        } else
        {
            SetClickableColliders(true);
        }
    }

    private void SetClickableColliders(bool state)
    {
        foreach (Collider col in clickableColliders)
        {
            col.enabled = state;
        }
    }

    private void HandleMovement()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = transform.forward;
            audioManager.PlaySFX(audioManager.footstep);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = -transform.forward;
            audioManager.PlaySFX(audioManager.footstep);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = -transform.right;
            audioManager.PlaySFX(audioManager.footstep);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = transform.right;
            audioManager.PlaySFX(audioManager.footstep);
        }

        if (direction != Vector3.zero && CanMove(direction))
        {
            lastSafePosition = transform.position; // Store last safe position
            transform.position += direction; // Instant movement
            StartCoroutine(MovementCooldown());
        }
    }

    private IEnumerator MovementCooldown()
    {
        canMove = false;
        isMoving = true;
        yield return new WaitForSeconds(movementCooldown);
        canMove = true;
        isMoving = false;
    }

    private void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(SmoothRotate(-90)); // Rotate left
        }
        else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.RightArrow))
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
            if (hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Enemy"))
            {
                return false; // Can't move in this direction
            }
        }
        return true; // Can move
    }

    private IEnumerator SmoothRotate(float angle)
    {
        isMoving = true;
        canMove = false;
        Quaternion originalRotation = transform.rotation;
        Vector3 targetAngle = transform.eulerAngles + Vector3.up * angle;
        Quaternion targetRotation = Quaternion.Euler(RoundVector(targetAngle));
        float step = 0f;

        while (step < 1f)
        {
            step += Time.deltaTime * rotationSpeed / 90f;
            transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, step);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(RoundVector(targetRotation.eulerAngles));
        isMoving = false;
        canMove = true;
    }

    private void UpdateCameraPosition()
    {
        if (playerCamera != null)
        {
            Vector3 desiredPosition = transform.position + transform.rotation * cameraOffset;
            playerCamera.transform.position = Vector3.MoveTowards(playerCamera.transform.position, desiredPosition, Time.deltaTime * cameraFollowSpeed);
            playerCamera.transform.rotation = Quaternion.Lerp(playerCamera.transform.rotation, transform.rotation, Time.deltaTime * cameraRotationSpeed);
        }
    }

    private Vector3 RoundVector(Vector3 vector)
    {
        return new Vector3(
            Mathf.Round(vector.x),
            Mathf.Round(vector.y),
            Mathf.Round(vector.z));
    }
    private void RoundPlayerPosition()
    {
        transform.position = new Vector3(
            Mathf.Round(transform.position.x),
            transform.position.y,
            transform.position.z
        );
    }

    private void CheckIfStuck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                transform.position = lastSafePosition; // Reset position if stuck inside an enemy
                break;
            }

            if (collider.CompareTag("Wall"))
            {
                transform.position = lastSafePosition; // Reset position if stuck inside a wall
                break;
            }
        }
    }
}
