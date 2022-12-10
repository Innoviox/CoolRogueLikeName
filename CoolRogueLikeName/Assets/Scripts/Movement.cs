using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public float acceleration; // Rate at which the player speeds up
    public float decceleration; // Rate at which the player slows down, changes the drag value of the Rigidbody
    public float baseMaxSpeed; // Max speed of the player
    public float jumpForce; // Increases the jump height of the player
    public float jumpCoolDown; // Amount of time between jumps
    // public bool doubleJump; // Enables the player to double jump
    public float dashForce; // Sets the distance of the player dash
    public float dashTime; // Sets the time the player is dashing for
    public KeyCode jumpKey; // Sets the jump key
    public KeyCode dashKey; // Sets the dash key
    public PowerupManager stats;

    public Slider jumpSlider;
    public Slider dashSlider;
    public TMP_Text maxJumpsText;
    public TMP_Text maxDashText;

    private Rigidbody rb;
    private bool dashLock;
    private bool jumpLock;

    private bool dash;
    private bool jump;
    public int numJumps = 2;
    public int numDashes = 1;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        dashLock = false;
        jumpLock = false;

        dash = false;
        jump = false;

        numJumps = 2;
        numDashes = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dashLock)
        {
            float hInput = Input.GetAxisRaw("Horizontal"); // Keeps track of left and right movement 
            float vInput = Input.GetAxisRaw("Vertical");   // Keeps track of forward and backwards movement
            if (hInput == 0)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
            }
            if (vInput == 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            }

            Vector3 accelerationDirection = new Vector3(hInput, 0, vInput).normalized; // This holds the direction the player is moving
            // Debug.Log("Direction: " + accelerationDirection.x + " " + accelerationDirection.y + " " + accelerationDirection.z);
            if (dash)
            {
                StartCoroutine(Dash(accelerationDirection));
                rb.velocity = Vector3.zero;
                dashSlider.value -= dashTime;
                dash = false;
                return;
            }

            if (jump)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpSlider.value -= jumpCoolDown;
                jump = false;
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

    void Update()
    {
        dashSlider.maxValue = dashTime * numDashes;
        dashSlider.value += Time.deltaTime;
        maxDashText.text = ((int)(dashSlider.value / dashTime)).ToString();

        jumpSlider.maxValue = jumpCoolDown * numJumps;
        jumpSlider.value += Time.deltaTime;
        maxJumpsText.text = ((int)(jumpSlider.value / jumpCoolDown)).ToString();

        dash = dash || Input.GetKeyDown(dashKey);
        jump = jump || Input.GetKeyDown(jumpKey) && CanJump();
    }

    bool CanJump()
    {
        return jumpSlider.value >= jumpCoolDown;
    }

    IEnumerator Dash(Vector3 direction)
    {
        dashLock = true;
        if (direction == Vector3.zero)
        {
            direction = new Vector3(0, 0, 1);
        }
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

    public void SetHud(Slider jumpSlider, Slider dashSlider, TMP_Text maxJumpsText, TMP_Text maxDashText)
    {
        this.jumpSlider = jumpSlider;
        this.dashSlider = dashSlider;
        this.maxJumpsText = maxJumpsText;
        this.maxDashText = maxDashText;

        dashSlider.maxValue = dashTime * numDashes;
        dashSlider.value = dashSlider.maxValue;

        jumpSlider.maxValue = jumpCoolDown * numJumps;
        jumpSlider.value = jumpSlider.maxValue;

        Debug.Log($"{dashSlider.value} / {dashSlider.maxValue} {jumpSlider.value} / {jumpSlider.maxValue}");
    }
}
