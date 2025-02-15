using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumberAnimation : MonoBehaviour
{
    public AnimationCurve opacityCurve;

    private TextMeshProUGUI tmp;
    private float time = 0f;

    private void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        tmp.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        time += Time.deltaTime;
    }
}
