using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerBody;
    [SerializeField] private float movementSpeed = 5, jump = 5;

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
        playerBody.velocity = new Vector3(hInput * movementSpeed,
                                          playerBody.velocity.y,
                                          vInput * movementSpeed);

        // Move player up
        if (Input.GetButtonDown("Jump"))
        {
            // Gets players velocity and changes velocity for y value. 
            playerBody.velocity = new Vector3(playerBody.velocity.x,
                                              jump,
                                              playerBody.velocity.z);
        }
    }
}
