using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGateBossRoom : MonoBehaviour
{
    public GameObject Boss;
    public Animator ironGateBossAnimator;
    public BoxCollider gateCollider;
    public float timeToWait = 4f;

    public AudioSource gateMovingSource;
    public AudioSource gateStopped;
    public bool bossKilled = false;

    private void Update()
    {
        if (Boss == null)
        {
            ironGateBossAnimator.SetTrigger("Killed");
            StartCoroutine(enableCollider(timeToWait));

            if (!bossKilled)
            {
                StartCoroutine(bossKilledSound());
            }

            enabled = false;
        }
    }

    IEnumerator enableCollider(float time)
    {
        yield return new WaitForSeconds(time);
        gateCollider.enabled = false;
    }

    IEnumerator bossKilledSound()
    {
        gateMovingSource.Play();
        yield return new WaitForSeconds(4f);
        gateStopped.Play();
    }
}
