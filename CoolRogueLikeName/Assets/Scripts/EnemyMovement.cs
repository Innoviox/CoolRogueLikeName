using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public Transform rightSide;      // Used to raycast 
    public Transform leftSide;       // from the enemy to the room

    public Rigidbody enemyBody;
    public float enemySpeed = 2.5f;   // Walking speed of enemy
    public float dodgeSpeed = 3;      // How fast the enemy moves when it dodges.

    public float maxDist = 5.0f;      // Distance enemy can be from player before moving. 
    private int allowedDistance;      // How close enemy can be to an object before changing directions (left/right) when dodging player bullets.
    private bool moveRight;           // Enemy moves right to dodge player bullets by default
    public float initialWait = 2.0f;  // How long to wait before the first call of Invoke in seconds.
    public float walkRate = 0.1f;     // How often the enemy will walk towards the player based on their distance. 
    

    // Start is called before the first frame update
    void Start()
    {
        enemyBody = GetComponent<Rigidbody>();
        allowedDistance = RandomDistance();
        moveRight = true;
        EnemyMove();
    }

    public virtual void EnemyMove()
    {
        InvokeRepeating(nameof(WalkTowardsPlayer), initialWait, walkRate);
    }

    // If the player is too far from the enemy on the x-z plane
    // Then the enemy will walk towards the player. 
    private void WalkTowardsPlayer()
    {
        // Get vector between player and enemy position
        var vectorTarget = player.position - transform.position;
        // only consider x-z plane distance
        vectorTarget.y = 0;

        // Move towards player if player is too far
        if (vectorTarget.magnitude >= maxDist)
        {
            // The enemy walks forward in the direction they are facing
            enemyBody.velocity = new Vector3(transform.forward.x * enemySpeed, 0, transform.forward.z * enemySpeed);
        }
        else
            DodgePlayerLineOfFire();
    }

    // Shoots ray out of player and moves left or right if enemy's body is hit the ray. 
    private void DodgePlayerLineOfFire()
    {
        // Get direcion player is looking
        Ray playerRay = new Ray(player.position, player.forward);

        // Send out a ray from player and store what it hits in hit. 
        if (Physics.Raycast(playerRay, out RaycastHit hit))
        {
            // If the player is looking at me then move based on which direction
            // I have space in.
            if (hit.transform.name == transform.name)
            {
                // Implemented Left and Right objects on enemy due to raycasting colliding with
                // The enemy object itself. I think layermask would problably be the next step to implement and
                // not use left/right objects. 

                // Get distance to objects on my right and left
                Ray rightOfEnemy = new Ray(rightSide.position, rightSide.right);
                Ray leftOfEnemy = new Ray(leftSide.position, leftSide.right * -1);

                RaycastHit rightHit;
                RaycastHit leftHit;

                Physics.Raycast(rightOfEnemy, out rightHit);
                Physics.Raycast(leftOfEnemy, out leftHit);

                // Move to my right
                if (moveRight)
                {
                    enemyBody.velocity = enemyBody.transform.right * dodgeSpeed;
                    if (rightHit.distance < allowedDistance)
                    {
                        moveRight = false;
                        allowedDistance = RandomDistance();
                    }
                }
                // Move to my left
                else
                {
                    // -1 so the enemy will move to the left rather than right
                    enemyBody.velocity = enemyBody.transform.right * -1 * dodgeSpeed;
                    if (leftHit.distance < allowedDistance)
                    {
                        moveRight = true;
                        allowedDistance = RandomDistance();
                    }
                }
            }
        }
    }
    private int RandomDistance()
    {
        return Random.Range(1, 4);
    }
}
