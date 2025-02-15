using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageNumberGenerator : MonoBehaviour
{
    public static DamageNumberGenerator current;
    public GameObject damageNumberPrefab;
    public Canvas playerUI;

    private void Awake()
    {
        current = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            CreatePopUp(Vector2.one, Random.Range(0, 100).ToString());
        }
    }

    public void CreatePopUp(Vector2 position, string text)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);
        var popup = Instantiate(damageNumberPrefab, playerUI.transform);
        RectTransform rectTransform = popup.GetComponent<RectTransform>();
        rectTransform.position = screenPosition;
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;

        Destroy(popup, 1f);
    }
}
