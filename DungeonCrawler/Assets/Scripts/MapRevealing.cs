using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MapRevealing : MonoBehaviour
{
    public Transform player;
    public MeshRenderer mapRender;
    public float mapRevealDistance = 1.1f;

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("PlayerReveal");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }


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
