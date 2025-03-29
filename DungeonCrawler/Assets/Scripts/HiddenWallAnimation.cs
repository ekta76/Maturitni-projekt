using System.Collections;
using UnityEngine;

public class HiddenWallAnimation : MonoBehaviour
{
    public Animator wallAnimator;
    public Animator buttonAnimator;
    public Movement playerMovement;
    public BoxCollider wallCollider;
    public LayerMask chainLayer;

    private float progress = 0f;
    private float currentWallSpeed = 0f;
    private float lastWallSpeed = 1f;
    private bool isFirstClick = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectButtonClick();
        }

        if (wallAnimator != null)
        {
            AnimatorStateInfo stateInfo = wallAnimator.GetCurrentAnimatorStateInfo(0);
            progress = Mathf.Clamp01(stateInfo.normalizedTime);

            if ((progress >= 1f && currentWallSpeed > 0) || (progress <= 0f && currentWallSpeed < 0))
            {
                currentWallSpeed = 0f;
                wallAnimator.SetFloat("Speed", currentWallSpeed);
            }

            wallCollider.enabled = progress < 0.99f;
        }
    }

    private void DetectButtonClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1f, chainLayer))
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