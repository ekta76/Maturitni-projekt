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
    private float currentGateSpeed = 0f;
    private float lastGateSpeed = 1f;
    private bool isFirstClick = true;
    private bool opening = false;

    public AudioSource gateStoppedSource;
    public AudioSource gateMovingSource;
    public AudioSource leverSource;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectLeverPull();
        }

        if (gateAnimator != null)
        {
            AnimatorStateInfo stateInfo = gateAnimator.GetCurrentAnimatorStateInfo(0);
            progress = Mathf.Clamp01(stateInfo.normalizedTime);

            if ((progress >= 1f && currentGateSpeed > 0) || (progress <= 0f && currentGateSpeed < 0))
            {
                currentGateSpeed = 0f;
                gateAnimator.SetFloat("Speed", currentGateSpeed);
                StartCoroutine(StopGateSoundAfterStop());
                StartCoroutine(PlayGateSoundEffectAfterStopping());
            }

            gateCollider.enabled = progress < 0.99f;
        }
    }

    private IEnumerator PlayGateSoundEffectAfterStopping()
    {
        yield return new WaitForEndOfFrame();

        if (gateAnimator != null)
        {
            gateStoppedSource.Play();
        }
    }

    private IEnumerator StopGateSoundAfterStop()
    {
        yield return new WaitForEndOfFrame();

        if (gateMovingSource != null && gateMovingSource.isPlaying)
        {
            gateMovingSource.Stop();
        }
    }

    private void DetectLeverPull()
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
                    PlayLeverAnimation();
                    if (leverSource != null)
                     {
                            leverSource.Play();
                     }
                    DisableMovementControllerTemporarily(0.2f);
                    isFirstClick = false;
                }
                else
                {
                   if (leverSource != null)
                     {
                            leverSource.Play();
                     }
                    opening = !opening;
                    PlayLeverAnimation();
                    ToggleWall();
                    DisableMovementControllerTemporarily(0.2f);
                }
            }
        }
        }
    }

    private void PlayLeverAnimation()
    {
        leverAnimator.SetBool("opening", opening);
    }

    public void ToggleWall()
    {
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

        if (currentGateSpeed != 0f && gateMovingSource != null && !gateMovingSource.isPlaying)
        {
            gateMovingSource.Play();
        }

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
