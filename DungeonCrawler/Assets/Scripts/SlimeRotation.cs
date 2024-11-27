using UnityEngine;

public class SlimeRotation : MonoBehaviour
{
    private Animator animator;
    private Camera mainCamera;

    void Start()
    {
        // Cache the Animator and Main Camera
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        UpdateAnimationParameters();
        HandleBillboard();
    }

    // Ensures the enemy always faces the camera
    private void HandleBillboard()
    {
        if (mainCamera == null) return;

        // Adjust the rotation to face the camera, ignoring vertical component
        Vector3 lookAtPosition = mainCamera.transform.position;
        lookAtPosition.y = transform.position.y;
        transform.LookAt(lookAtPosition);
    }

    // Updates the animator parameters based on the camera's relative position
    private void UpdateAnimationParameters()
    {
        if (mainCamera == null || animator == null) return;

        // Get the relative direction from the enemy to the camera
        Vector3 objectToCamera = mainCamera.transform.position - transform.position;
        objectToCamera.y = 0f; // Ignore vertical component
        objectToCamera.Normalize();

        // Get the enemy's original forward direction (ignoring LookAt billboard effect)
        Vector3 enemyForward = transform.parent != null ? transform.parent.forward : Vector3.forward;
        enemyForward.y = 0f;
        enemyForward.Normalize();

        // Calculate the signed angle to determine relative direction
        float angle = Vector3.SignedAngle(enemyForward, objectToCamera, Vector3.up);

        // Determine which animation to play based on the angle
        Vector2 move = DetermineMoveDirection(angle);

        // Pass the values to the Animator
        animator.SetFloat("moveX", move.x);
        animator.SetFloat("moveY", move.y);
    }

    // Determines the move direction for the animator based on the angle
    private Vector2 DetermineMoveDirection(float angle)
    {
        if (angle > -45f && angle <= 45f) // Front
            return new Vector2(0f, -1f);
        else if (angle > 45f && angle <= 135f) // Right
            return new Vector2(1f, 0f);
        else if (angle > -135f && angle <= -45f) // Left
            return new Vector2(-1f, 0f);
        else // Back
            return new Vector2(0f, 1f);
    }
}
