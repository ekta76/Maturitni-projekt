using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateAnimationLever : MonoBehaviour
{
    public Animator gateAnimator;  // Reference to the Wall's Animator
    public Animator leverAnimator;  // Reference to the Button's Animator
    public Movement playerMovement; // Reference to the Player's Movement script
    public BoxCollider gateCollider; // Reference to the Wall's BoxCollider
    public LayerMask chainLayer; // Layer mask for button detection
    private Grid gridManager;

    private float progress = 0f; // Tracks animation progress (0-1)
    private float currentWallSpeed = 0f; // Current animation speed (0, 1, or -1)
    private float lastWallSpeed = 1f; // Tracks last movement direction
    private bool isFirstClick = true;
    private bool opening = false;

    private void Start()
    {
        gridManager = FindObjectOfType<Grid>();
    }

    private void Update()
    {
        // Check for button click
        if (Input.GetMouseButtonDown(0))
        {
            DetectButtonClick();
        }

        // Update animation progress and handle boundaries
        if (gateAnimator != null)
        {
            AnimatorStateInfo stateInfo = gateAnimator.GetCurrentAnimatorStateInfo(0);
            progress = Mathf.Clamp01(stateInfo.normalizedTime);

            // Stop animation at boundaries
            if ((progress >= 1f && currentWallSpeed > 0) || (progress <= 0f && currentWallSpeed < 0))
            {
                currentWallSpeed = 0f;
                gateAnimator.SetFloat("Speed", currentWallSpeed);
            }

            // Update collider state
            gateCollider.enabled = progress < 0.99f; // Adjust threshold as needed
        }
    }

    private void DetectButtonClick()
    {
        AnimatorStateInfo stateInfo = leverAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("LeverClosed") || stateInfo.IsName("WoodenLeverOpeningIdle")) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1f, chainLayer))
        {
            if (hit.collider.CompareTag("Lever") && hit.collider.gameObject == leverAnimator.gameObject)
            {
                if (isFirstClick)
                {
                    opening = !opening;
                    ToggleWall();
                    ToggleWall();
                    PlayButtonAnimation();
                    DisableMovementControllerTemporarily(0.2f);
                    isFirstClick = false;
                }
                else
                {
                    opening = !opening;
                    PlayButtonAnimation();
                    ToggleWall();
                    DisableMovementControllerTemporarily(0.2f);
                }
            }
        }
        }
    }

    private void PlayButtonAnimation()
    {
        leverAnimator.SetBool("opening", opening);
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
        gateAnimator.SetFloat("Speed", currentWallSpeed);

        // Ensure the animation starts from the correct normalized time
        float normalizedTime = GetNormalizedTime();
        gateAnimator.Play("GateAnimation", 0, normalizedTime);
    }

    private float GetNormalizedTime()
    {
        AnimatorStateInfo stateInfo = gateAnimator.GetCurrentAnimatorStateInfo(0);
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

    public void AnimationUpdateGrid() // Called via animation event
    {
        if (gridManager != null)
        {
            gridManager.UpdateGridFromAnimation();
        }
    }
}
