using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private float inputX, inputZ;     // player's movement inputs
    private float speed;              // the speed at which the player moves
    private void Awake()
    {
        GameDataHandler.OnDataFetchSuccess += SetupPlayerMovement;
    }

    void Start()
    {
        // initialize input values to zero when the game starts
        inputX = 0f;
        inputZ = 0f;
    }

    void Update()
    {
        // get the horizontal and vertical input from the player
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");

        // create a vector that represents the direction the player wants to move in
        Vector3 movementDirection = new(inputX, 0f, inputZ);

        // move the player in the direction they’re inputting using the set speed
        transform.Translate(speed * Time.deltaTime * movementDirection, Space.World);
    }

    private void OnDestroy()
    {
        // unsubscribe from the event to avoid potential memory leaks
        GameDataHandler.OnDataFetchSuccess -= SetupPlayerMovement;
    }

    private void SetupPlayerMovement()
    {
        // get the speed value from gasmedatahandler and apply it
        speed = GameDataHandler.PlayerSpeed;
    }
}

