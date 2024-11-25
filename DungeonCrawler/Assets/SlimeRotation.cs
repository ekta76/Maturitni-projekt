using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRotation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Rotate the object to face the camera (billboarding)
        transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);

        // Get the camera's forward direction relative to the grid
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0; // Ignore vertical component
        cameraForward.Normalize();

        // Calculate the relative direction between the object and the camera
        Vector3 objectToCamera = Camera.main.transform.position - transform.position;
        objectToCamera.y = 0; // Ignore vertical component
        objectToCamera.Normalize();

        // Determine which direction the camera is relative to the object
        float moveX = 0f;
        float moveY = 0f;

        if (Vector3.Dot(objectToCamera, Vector3.forward) > 0.7f) // Camera is in front of the object
        {
            moveX = 0f;
            moveY = -1f;
        }
        else if (Vector3.Dot(objectToCamera, Vector3.back) > 0.7f) // Camera is behind the object
        {
            moveX = 0f;
            moveY = 1f;
        }
        else if (Vector3.Dot(objectToCamera, Vector3.right) > 0.7f) // Camera is to the right of the object
        {
            moveX = -1f;
            moveY = 0f;
        }
        else if (Vector3.Dot(objectToCamera, Vector3.left) > 0.7f) // Camera is to the left of the object
        {
            moveX = 1f;
            moveY = 0f;
        }

        // Pass the values to the Animator
        animator.SetFloat("moveX", moveX);
        animator.SetFloat("moveY", moveY);
    }
}
