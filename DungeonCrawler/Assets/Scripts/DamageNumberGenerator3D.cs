using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumberGenerator3D : MonoBehaviour
{
    public static DamageNumberGenerator3D current;
    public GameObject damageNumberPrefab;

    private void Awake()
    {
        current = this;
    }

    public void CreatePopUp(Vector3 position, string text)
    {
        var popup = Instantiate(damageNumberPrefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;

        Destroy(popup, 1f);
    }
}
