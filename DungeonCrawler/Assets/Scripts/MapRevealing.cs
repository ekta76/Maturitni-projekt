using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MapRevealing : MonoBehaviour
{
    public Transform player;
    public MeshRenderer mapRender;
    public float mapRevealDistance = 0f;

    private void Start()
    {
        mapRender = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < mapRevealDistance)
        {
            mapRender.enabled = true;
        }
    }
}
