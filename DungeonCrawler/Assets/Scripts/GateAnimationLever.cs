using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateAnimationLever : MonoBehaviour
{
    public Animator gateAnimator;
    public Animator leverAnimator;
    public Movement playerMovement;
    public BoxCollider gateCollider;
    public LayerMask chainLayer;

    private float progress = 0f;
    private float currentWallSpeed = 0f;
    private float lastWallSpeed = 1f;
    private bool isFirstClick = true;
    private bool opening = false;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectButtonClick();
        }

        if (gateAnimator != null)
        {
            AnimatorStateInfo stateInfo = gateAnimator.GetCurrentAnimatorStateInfo(0);
            progress = Mathf.Clamp01(stateInfo.normalizedTime);

            if ((progress >= 1f && currentWallSpeed > 0) || (progress <= 0f && currentWallSpeed < 0))
            {
                currentWallSpeed = 0f;
                gateAnimator.SetFloat("Speed", currentWallSpeed);
            }

            gateCollider.enabled = progress < 0.99f;
        }
    }

    private void DetectButtonClick()
    {
        AnimatorStateInfo stateInfo = leverAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("LeverClosed") || stateInfo.IsName("WoodenLeverOpeningIdle")) 
        {
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
}
