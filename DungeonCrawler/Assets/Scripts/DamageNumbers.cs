using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumbers : MonoBehaviour
{
    public float lifetime = 1f;
    public float floatSpeed = 1f;
    public float randomOffset = 0.5f; // Random positioning range

    private TextMeshPro text;
    private Vector3 moveDirection;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        moveDirection = new Vector3(0, 1, 0); // Move upwards
        Destroy(gameObject, lifetime);
    }

    public void Initialize(int damage, Vector3 position, bool isEnemyHit = false)
    {
        transform.position = position;
        text = GetComponent<TextMeshPro>();
        text.text = damage.ToString();

        if (isEnemyHit)
        {
            // Apply slight random offset in screen space
            transform.position += new Vector3(Random.Range(-randomOffset, randomOffset), Random.Range(-randomOffset, randomOffset), 0);
        }
    }

    void Update()
    {
        transform.position += moveDirection * floatSpeed * Time.deltaTime;
        text.alpha -= Time.deltaTime / lifetime; // Fade out effect
    }
}
