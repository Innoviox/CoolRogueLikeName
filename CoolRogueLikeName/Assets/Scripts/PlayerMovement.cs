using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerBody;
    [SerializeField] private float baseMovementSpeed = 5, jump = 5;
    public PowerupManager stats;

    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
        {
            return;
        }

        float hInput = Input.GetAxis("Horizontal"); // Keeps track of left and right movement 
        float vInput = Input.GetAxis("Vertical");   // Keeps track of forward and backwards movement

        // Updates players movement left/right and forward/backwards.
        // Weird approach prevents moving faster diagonally.
        Vector3 temp = Vector3.ClampMagnitude(new Vector3(hInput, 0, vInput), 1);
        temp *= baseMovementSpeed * stats.playerMoveSpeedFactor;
        temp.y = playerBody.velocity.y;
        playerBody.velocity = temp;

        // Move player up
        if (Input.GetButtonDown("Jump"))
        {
            // Gets players velocity and changes velocity for y value. 
            playerBody.velocity = new Vector3(playerBody.velocity.x,
                                              jump,
                                              playerBody.velocity.z);
        }
    }

    // Testing
    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entered Trigger");
    }
}
