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
        if (Boss == null && !bossKilled)
        {
            ironGateBossAnimator.SetTrigger("Killed");
            bossKilled = true;
            StartCoroutine(bossKilledEffect());
        }
    }

    IEnumerator bossKilledEffect()
    {
        gateMovingSource.Play();
        yield return new WaitForSeconds(4f);
        gateCollider.enabled = false;
        gateStopped.Play();
        enabled = false;
    }
}
