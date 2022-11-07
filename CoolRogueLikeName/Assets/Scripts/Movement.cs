using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float acceleration; // Rate at which the player speeds up
    public float decceleration; // Rate at which the player slows down, changes the drag value of the Rigidbody
    public float maxSpeed; // Max speed of the player
    public float jumpForce; // Increases the jump height of the player
    public float jumpCoolDown; // Amount of time between jumps
    public bool doubleJump; // Enables the player to double jump
    public float dashDistance; // Sets the distance of the player dash
    public float dashTime; // Sets the time the player is dashing for

    private Rigidbody rb;
    private bool dashLock;
    private bool jumpLock;
    private bool doubleJumpLock;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dashLock = false;
        jumpLock = false;
        doubleJumpLock = false;
        rb.drag = decceleration;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dashLock) {
            float hInput = Input.GetAxis("Horizontal"); // Keeps track of left and right movement 
            float vInput = Input.GetAxis("Vertical");   // Keeps track of forward and backwards movement

            Vector3 accelerationDirection = new Vector3(hInput, 0, vInput).normalized; // This holds the direction the player is moving
            if (Input.GetKeyDown("space")){
                dashLock = true;
                StartCoroutine(Dash(accelerationDirection));
                rb.velocity = Vector3.zero;
                return;
            }
            if (Input.GetKeyDown("b"))
            {
                if (!jumpLock)
                {
                    StartCoroutine(Jump());
                    return;
                } else if (!doubleJumpLock && doubleJump)
                {
                    doubleJumpLock = true;
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
            }
            rb.AddForce(accelerationDirection * acceleration, ForceMode.Acceleration); // Adds the acceleration to the player

            /* Keeps the velocity capped at maxSpeed */
            Vector3 velocity = rb.velocity;
            if (velocity.magnitude > maxSpeed)
            {
                rb.velocity = velocity.normalized * maxSpeed;
            }

        }
    }

    IEnumerator Dash(Vector3 direction)
    {
        Vector3 currentPosition = GetComponent<Transform>().position;
        while(true)
        {
            rb.position += direction * (dashDistance * Time.fixedDeltaTime / dashTime);
            if(Vector3.Distance(currentPosition, rb.position) > dashDistance)
            {
                dashLock = false;
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator Jump()
    {
        jumpLock = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        yield return new WaitForSeconds(jumpCoolDown);
        jumpLock = false;
        doubleJumpLock = false;
        yield break;
    }
}
