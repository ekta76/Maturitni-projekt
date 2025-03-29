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

            if (isPlayerCamera)
            {
                Time.timeScale = 1f;
            } 
            else
            {
                Time.timeScale = 0f;
            }
        }

        if (!isPlayerCamera)
        {
            Cursor.visible = false;
        }
    }
}
