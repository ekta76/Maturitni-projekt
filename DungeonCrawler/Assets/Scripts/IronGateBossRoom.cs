using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGateBossRoom : MonoBehaviour
{
    public GameObject Boss;
    public Animator ironGateBossAnimator;
    public BoxCollider gateCollider;
    public float timeToWait = 4f;

    private void Update()
    {
        if (Boss == null)
        {
            ironGateBossAnimator.SetTrigger("Killed");
            StartCoroutine(enableCollider(timeToWait));
            enabled = false;
        }
    }

    IEnumerator enableCollider(float time)
    {
        yield return new WaitForSeconds(time);
        gateCollider.enabled = false;

    }
}
