using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float acceleration; // Rate at which the player speeds up
    public float decceleration; // Rate at which the player slows down, changes the drag value of the Rigidbody
    public float baseMaxSpeed; // Max speed of the player
    public float jumpForce; // Increases the jump height of the player
    public float jumpCoolDown; // Amount of time between jumps
    public bool doubleJump; // Enables the player to double jump
    public float dashForce; // Sets the distance of the player dash
    public float dashTime; // Sets the time the player is dashing for
    public KeyCode jumpKey; // Sets the jump key
    public KeyCode dashKey; // Sets the dash key
    public PowerupManager stats;

    private Rigidbody rb;
    private bool dashLock;
    private bool jumpLock;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dashLock = false;
        jumpLock = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dashLock)
        {
            float hInput = Input.GetAxisRaw("Horizontal"); // Keeps track of left and right movement 
            float vInput = Input.GetAxisRaw("Vertical");   // Keeps track of forward and backwards movement
            if(hInput == 0)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            }
            if(vInput == 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            }

            Vector3 accelerationDirection = new Vector3(hInput, 0, vInput).normalized; // This holds the direction the player is moving
            // Debug.Log("Direction: " + accelerationDirection.x + " " + accelerationDirection.y + " " + accelerationDirection.z);
            if (Input.GetKeyDown(dashKey))
            {
                StartCoroutine(Dash(accelerationDirection));
                rb.velocity = Vector3.zero;
                return;
            }
            if (Input.GetKeyDown(jumpKey) && !jumpLock)
            {
                StartCoroutine(Jump());
                return;
            }
                rb.AddForce(accelerationDirection * acceleration, ForceMode.Acceleration); // Adds the acceleration to the player

                /* Keeps the velocity capped at maxSpeed */

                Vector3 velocity = rb.velocity;
                Vector2 horizontalVelocity = new Vector2(velocity.x, velocity.z);
                if (horizontalVelocity.magnitude > baseMaxSpeed * stats.playerMoveSpeedFactor)
                {
                    horizontalVelocity = horizontalVelocity.normalized * baseMaxSpeed * stats.playerMoveSpeedFactor;
                }
                rb.velocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.y);
        }

    }

    IEnumerator Dash(Vector3 direction)
    {
        dashLock = true;
        rb.AddForce(direction * dashForce, ForceMode.Impulse);
        yield return new WaitForSeconds(dashTime);
        dashLock = false;
        yield break;
        //while(true)
        // {
        //rb.position += direction * (dashDistance * Time.fixedDeltaTime / dashTime);
        //if(Vector3.Distance(currentPosition, rb.position) > dashDistance)
        //{
        //    dashLock = false;
        //    yield break;
        //}
        //yield return null;
        //}
    }

    IEnumerator Jump()
    {
        jumpLock = true;
        // rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        // rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        yield return null;
        if (doubleJump)
        {
            StartCoroutine(DoubleJump());
        }
        yield return new WaitForSeconds(jumpCoolDown);
        jumpLock = false;
        yield break;

    }

    IEnumerator DoubleJump()
    {
        while (jumpLock)
        {
            if (Input.GetKeyDown(jumpKey))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                yield break;
            }
            yield return null;
        }
        yield break;
    }
}
