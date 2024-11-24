using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SerializeField] Transform mainTransform;
[SerializeField] Animator animator;
[SerializeField] SpriteRenderer SpriteRenderer;
public class SlimeBillboard : MonoBehaviour
{
    Vector3 camForwardVector = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z);

    float signedAngle = Vector3.SignedAngle(mainTransform.forward, camForwardVector, Vector3.up);
}
