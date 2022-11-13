using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleGunMovement : EnemyMovement
{
    protected void Awake()
    {
        enemyBody = GetComponent<Rigidbody>();
        allowedDistance = RandomDistance();
        moveRight = true;
        walkRate = 0.2f;
    }
    // Start is called before the first frame update
    protected void Start()
    {
        StartCoroutine(EnemyMove());
    }

    protected void Update()
    {
        // The enemy will constantly look at the player.
        transform.LookAt(player);
    }

    /// <summary>
    /// Move Towards the player until player is close enough and then
    /// begin to dodge the players line of fire. 
    /// </summary>
    public IEnumerator EnemyMove()
    {
        float maxDist = 4.0f;

        while (true)
        {
            // Check if player is close enough to dodge or if player has moved from out of range.
            if (EnemyHelpers.DistToPlayer(player, transform) > maxDist)
            {
                WalkTowardsPlayer(maxDist);
            }
            else
            {
                DodgePlayerLineOfFire();
            }

            yield return new WaitForSeconds(walkRate);
        }
    }
}
