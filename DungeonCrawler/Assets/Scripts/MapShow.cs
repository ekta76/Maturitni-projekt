using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapShow : MonoBehaviour
{
    public GameObject playerCamera;
    public GameObject mapCamera;
    public GameObject player;
    private Movement movement;
    public GameObject uiElements;

    private bool isPlayerCamera = true;

    private void Start()
    {
        movement = player.GetComponent<Movement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            isPlayerCamera = !isPlayerCamera;

            playerCamera.SetActive(isPlayerCamera);
            mapCamera.SetActive(!isPlayerCamera);

            Cursor.visible = isPlayerCamera;
            movement.enabled = isPlayerCamera;
            uiElements.SetActive(isPlayerCamera);

            Time.timeScale = isPlayerCamera ? 1f : 0f;
        }
    }
}
