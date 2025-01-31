using System.Collections;
using UnityEngine;

public class HiddenWAllAnimatio : MonoBehaviour
{
    public Animator wallAnimator;  // Reference to the Gate's Animator
    public Animator buttonAnimator;  // Reference to the Chain's Animator
    public Movement playerMovement; // Reference to the Player's Movement script
    public BoxCollider wallCollider; // Reference to the Gate's BoxCollider
    public LayerMask chainLayer; // Layer mask for chain detection

    private bool isWallOpening = false; // Tracks if the gate is currently opening
    private bool isWallClosing = false; // Tracks if the gate is currently closing
    private float progress = 0f; // Progress of the gate (0 = closed, 1 = open)
    private float currentWallSpeed = 0f; // Tracks the current speed of the gate animation

    private void Update()
    {
        // Check for interaction with the chain
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            DetectChainClick();
        }

        // Ensure the playerMovement is assigned manually from the Inspector
        if (playerMovement != null)
        {
            Debug.Log("GateController script is referenced.");
        }
    }

    private void DetectChainClick()
    {
        // Cast a ray from the camera to detect chain clicks
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Raycast with the LayerMask to filter out objects that are not in the chainLayer
        float maxDistance = 1f; // Set the maximum range
        if (Physics.Raycast(ray, out hit, maxDistance, chainLayer))
        {
            // Check if the hit object is the chain
            if (hit.collider.CompareTag("HiddenButton"))
            {
                PlayButtonAnimation();
                ToggleWall(); // Toggle gate logic
                DisablePlayerControllerTemporarily(0.2f);
            }
        }
    }

    private void PlayButtonAnimation()
    {
        buttonAnimator.SetTrigger("Push"); // Trigger the chain pull animation
    }

    public void DisablePlayerControllerTemporarily(float disableDuration)
    {
        if (playerMovement != null)
        {
            // Disable the script
            playerMovement.enabled = false;
            Debug.Log("GateController script has been disabled.");

            // Start the coroutine to re-enable the script after a delay
            StartCoroutine(ReenableWallControllerAfterDelay(disableDuration));
        }
    }

    // Coroutine to re-enable the PlayerMovement script after the specified delay
    private IEnumerator ReenableWallControllerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified time
        if (playerMovement != null)
        {
            playerMovement.enabled = true; // Re-enable the script
        }
        Debug.Log("GateController script has been re-enabled.");
    }

    public void ToggleWall()
    {
        // Reverse the direction of the gate if it is currently moving
        if (isWallOpening || isWallClosing)
        {
            isWallOpening = !isWallOpening;
            isWallClosing = !isWallClosing;

            // Reverse direction while keeping progress intact
            float currentNormalizedTime = GetNormalizedTime();
            currentWallSpeed = isWallOpening ? 1f : -1f; // Track the current speed
            wallAnimator.SetFloat("Speed", currentWallSpeed); // Set the speed
            wallAnimator.Play("GateAnimation", 0, currentNormalizedTime); // Continue from current progress
        }
        else
        {
            // Start opening or closing based on progress
            if (progress >= 1f) // Fully open
            {
                isWallOpening = false;
                isWallClosing = true;
                currentWallSpeed = -1f; // Closing speed
                wallAnimator.SetFloat("Speed", currentWallSpeed); // Set speed to closing
            }
            else if (progress <= 0f) // Fully closed
            {
                isWallOpening = true;
                isWallClosing = false;
                currentWallSpeed = 1f; // Opening speed
                wallAnimator.SetFloat("Speed", currentWallSpeed); // Set speed to opening
            }

            // Play from the current progress
            float currentNormalizedTime = GetNormalizedTime();
            wallAnimator.Play("GateAnimation", 0, currentNormalizedTime);
        }
    }

    private float GetNormalizedTime()
    {
        // Get the normalized time of the current animation state
        AnimatorStateInfo stateInfo = wallAnimator.GetCurrentAnimatorStateInfo(0);
        return Mathf.Repeat(stateInfo.normalizedTime, 1f); // Keep it between 0 and 1
    }

    // Called by the gate animation via Animation Events
    public void UpdateProgress(float currentProgress)
    {
        progress = Mathf.Clamp01(currentProgress);

        // Stop movement at the boundaries
        if (progress >= 1f || progress <= 0f)
        {
            isWallOpening = false;
            isWallClosing = false;
            wallAnimator.SetFloat("Speed", 0f); // Pause animation
        }
    }

    public void WallOpened()
    {
        wallCollider.enabled = false;
    }

    public void WallClosed()
    {
        wallCollider.enabled = true;
    }
}

