using System.Collections;
using UnityEngine;

public class HiddenWallAnimation : MonoBehaviour
{
    public Animator wallAnimator;  // Reference to the Wall's Animator
    public Animator buttonAnimator;  // Reference to the Button's Animator
    public Movement playerMovement; // Reference to the Player's Movement script
    public BoxCollider wallCollider; // Reference to the Wall's BoxCollider
    public LayerMask chainLayer; // Layer mask for button detection

    private float progress = 0f; // Tracks animation progress (0-1)
    private float currentWallSpeed = 0f; // Current animation speed (0, 1, or -1)
    private float lastWallSpeed = 1f; // Tracks last movement direction
    private bool isFirstClick = true;

    private void Update()
    {
        // Check for button click
        if (Input.GetMouseButtonDown(0))
        {
            DetectButtonClick();
        }

        // Update animation progress and handle boundaries
        if (wallAnimator != null)
        {
            AnimatorStateInfo stateInfo = wallAnimator.GetCurrentAnimatorStateInfo(0);
            progress = Mathf.Clamp01(stateInfo.normalizedTime);

            // Stop animation at boundaries
            if ((progress >= 1f && currentWallSpeed > 0) || (progress <= 0f && currentWallSpeed < 0))
            {
                currentWallSpeed = 0f;
                wallAnimator.SetFloat("Speed", currentWallSpeed);
            }

            // Update collider state
            wallCollider.enabled = progress < 0.99f; // Adjust threshold as needed
        }
    }

    private void DetectButtonClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, chainLayer))
        {
            if (hit.collider.CompareTag("Button") && hit.collider.gameObject == buttonAnimator.gameObject)
            {
                if (isFirstClick)
                {
                    ToggleWall();
                    ToggleWall();
                    PlayButtonAnimation();
                    DisableMovementControllerTemporarily(0.2f);
                    isFirstClick = false;
                }
                else
                {
                    PlayButtonAnimation();
                    ToggleWall();
                    DisableMovementControllerTemporarily(0.2f);
                }
            }
        }
    }

    private void PlayButtonAnimation()
    {
        buttonAnimator.SetTrigger("Push");
    }

    public void ToggleWall()
    {
        // Reverse direction or start movement
        if (currentWallSpeed != 0f)
        {
            currentWallSpeed *= -1;
        }
        else
        {
            currentWallSpeed = -lastWallSpeed;
        }

        lastWallSpeed = currentWallSpeed;
        wallAnimator.SetFloat("Speed", currentWallSpeed);

        // Ensure the animation starts from the correct normalized time
        float normalizedTime = GetNormalizedTime();
        wallAnimator.Play("WallAnimation", 0, normalizedTime);
    }

    private float GetNormalizedTime()
    {
        AnimatorStateInfo stateInfo = wallAnimator.GetCurrentAnimatorStateInfo(0);
        return Mathf.Clamp01(stateInfo.normalizedTime);
    }

    public void DisableMovementControllerTemporarily(float duration)
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            StartCoroutine(ReenableMovementController(duration));
        }
    }

    private IEnumerator ReenableMovementController(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }
}