using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public Transform rightSide;     // Used to raycast 
    public Transform leftSide;      // from the enemy to the room

    public Rigidbody enemyBody;
    public float baseEnemySpeed = 2.5f; // Walking speed of enemy
    public float dodgeSpeed = 3;    // How fast the enemy moves when it dodges.

    protected int allowedDistance;  // How close enemy can be to an object before changing directions (left/right) when dodging player bullets.
    protected bool moveRight;       // Enemy moves right to dodge player bullets by default

    public float walkRate;          // How often the enemy will walk towards the player based on their distance. 

    public PowerupManager stats;

    /// <summary>
    /// If the player is too far from the enemy on the x-z plane
    /// Then the enemy will walk towards the player. 
    /// </summary>
    protected virtual bool WalkTowardsPlayer(float maxDist)
    {
        bool tooFar = EnemyHelpers.DistToPlayer(player, transform) >= maxDist;

        if (tooFar)
        {
            // The enemy walks forward in the direction they are facing
            enemyBody.velocity = new Vector3(transform.forward.x * baseEnemySpeed * stats.enemyMoveSpeedFactor, 0, transform.forward.z * baseEnemySpeed * stats.enemyMoveSpeedFactor);
        }
        return tooFar;
    }

    /// <summary>
    /// Shoots ray out of player and moves left or right if enemy's body is hit the ray. 
    /// </summary>
    protected virtual void DodgePlayerLineOfFire()
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
    protected int RandomDistance()
    {
        return Random.Range(1, 4);
    }
}
