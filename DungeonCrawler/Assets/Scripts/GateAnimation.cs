using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GateAnimation : MonoBehaviour
{
    public Animator gateAnimator;  // Reference to the Gate's Animator
    public Animator leftChainAnimator;  // Reference to the Left Chain's Animator
    public Animator rightChainAnimator; // Reference to the Right Chain's Animator
    public Movement playerMovement; // Reference to the Player's Movement script
    public BoxCollider gateCollider; // Reference to the Gate's BoxCollider
    public LayerMask chainLayer; // Layer mask for chain detection
    private Grid gridManager;

    private float progress = 0f; // Tracks animation progress (0-1)
    private float currentGateSpeed = 0f; // Current animation speed (0, 1, or -1)
    private float lastGateSpeed = 1f; // Tracks last movement direction
    private bool isFirstClick = true;

    private void Start()
    {
        gridManager = FindObjectOfType<Grid>();
    }

    private void Update()
    {
        // Check for interaction with the chains
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            DetectChainClick();
        }

        // Update animation progress and handle boundaries
        if (gateAnimator != null)
        {
            AnimatorStateInfo stateInfo = gateAnimator.GetCurrentAnimatorStateInfo(0);
            progress = Mathf.Clamp01(stateInfo.normalizedTime);

            // Stop animation at boundaries
            if ((progress >= 1f && currentGateSpeed > 0) || (progress <= 0f && currentGateSpeed < 0))
            {
                currentGateSpeed = 0f;
                gateAnimator.SetFloat("Speed", currentGateSpeed);
            }

            // Update collider state
            gateCollider.enabled = progress < 0.99f; // Adjust threshold as needed
        }
    }

    private void DetectChainClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1f, chainLayer))
        {
            if (hit.collider.CompareTag("FrontChain") && hit.collider.gameObject == leftChainAnimator.gameObject)
            {
                if (isFirstClick)
                {
                    ToggleGate();
                    ToggleGate();
                    PlayChainAnimation(leftChainAnimator);
                    DisablePlayerControllerTemporarily(0.2f);
                    isFirstClick = false;
                }
                else
                {
                    PlayChainAnimation(leftChainAnimator);
                    ToggleGate();
                    DisablePlayerControllerTemporarily(0.2f);
                }
            }
            else if (hit.collider.CompareTag("BackChain") && hit.collider.gameObject == rightChainAnimator.gameObject)
            {
                if (isFirstClick)
                {
                    ToggleGate();
                    ToggleGate();
                    PlayChainAnimation(rightChainAnimator);
                    DisablePlayerControllerTemporarily(0.2f);
                    isFirstClick = false;
                }
                else
                {
                    PlayChainAnimation(rightChainAnimator);
                    ToggleGate();
                    DisablePlayerControllerTemporarily(0.2f);
                }
            }
        }
    }

    private void PlayChainAnimation(Animator chainAnimator)
    {
        chainAnimator.SetTrigger("Pull"); // Trigger the chain pull animation
    }

    public void ToggleGate()
    {
        // Reverse direction or start movement
        if (currentGateSpeed != 0f)
        {
            currentGateSpeed *= -1;
        }
        else
        {
            currentGateSpeed = -lastGateSpeed;
        }

        lastGateSpeed = currentGateSpeed;
        gateAnimator.SetFloat("Speed", currentGateSpeed);

        // Ensure the animation starts from the correct normalized time
        float normalizedTime = GetNormalizedTime();
        gateAnimator.Play("GateAnimation", 0, normalizedTime);
    }

    private float GetNormalizedTime()
    {
        AnimatorStateInfo stateInfo = gateAnimator.GetCurrentAnimatorStateInfo(0);
        return Mathf.Clamp01(stateInfo.normalizedTime);
    }

    public void DisablePlayerControllerTemporarily(float duration)
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            StartCoroutine(ReenablePlayerController(duration));
        }
    }

    private IEnumerator ReenablePlayerController(float delay)
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