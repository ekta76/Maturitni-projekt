using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerMovement : MonoBehaviour
{

    public KeyCode forward = KeyCode.W;
    public KeyCode back = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode turnLeft = KeyCode.Q;
    public KeyCode turnRight = KeyCode.E;

    Movement movement;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        /*
        if(Input.GetKeyDown(forward)) movement.MoveForward();
        if (Input.GetKeyDown(back)) movement.MoveBackwards();
        if (Input.GetKeyDown(left)) movement.MoveLeft();
        if (Input.GetKeyDown(right)) movement.MoveRight();
        if (Input.GetKeyDown(turnLeft)) movement.RotateLeft();
        if (Input.GetKeyDown(turnRight)) movement.RotateRight();
        */
    }

}

