using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : EnemyMovement
{
    public Transform center;
    private BossAttack attack;
    public Coroutine aimCo = null;

    // Currently Using updates to Position based movement so boss can be kinematic
    // and the player cannot simply push the boss around with their rigidbody. 

    private void Awake()
    {
        attack = transform.GetComponent<BossAttack>();
        walkRate = 0.01f; // 0.2f For velocity based
        enemyBody = GetComponent<Rigidbody>();
    }

    public IEnumerator MoveToPlayer()
    {
        float maxDist = 9f;
        while (true)
        {
            // Check if player is close enough, otherwise walk towards player.
            if (EnemyHelpers.DistToPlayer(player, transform) > maxDist)
            {
                // Transform position based movement
                Vector3 temp = 20 * Time.deltaTime * (player.position - transform.position).normalized;
                temp.y = 0;
                transform.position += temp;
                // End transform based movement

                // Velocity based
                //WalkTowardsPlayer(maxDist);
            }
            //else
            //  enemyBody.velocity = new Vector3(0, 0, 0);

            yield return new WaitForSeconds(walkRate);
        }
    }

    /// <summary>
    /// Starts the enemies aim coroutine
    /// </summary>
    public void StartAim(Transform target)
    {
        aimCo = StartCoroutine(EnemyAim(target));
    }

    /// <summary>
    /// Stops enemies aim coroutine
    /// </summary>
    public void StopAim()
    {
        StopCoroutine(aimCo);
        aimCo = null;
    }

    public IEnumerator MoveToCenter()
    {
        StartAim(center);

        while (!inRange())
        {
            // For velocity 
            // enemyBody.velocity = new Vector3(transform.forward.x * enemySpeed, 0, transform.forward.z * enemySpeed);

            // Transform position based movement
            Vector3 temp = 20 * Time.deltaTime * (center.position - transform.position).normalized;
            temp.y = 0;
            transform.position += temp;
            // End transform based movement

            yield return new WaitForSeconds(walkRate);
        }
        // enemyBody.velocity = new Vector3(0, 0, 0);

        StopAim();
        StartCoroutine(attack.WaveAttack());
        yield return null;
    }

    /// <summary>
    /// Check if boss is within an accepted amount of range of the center of the room.
    /// </summary>
    /// <returns></returns> True if in range, false otherwise
    bool inRange()
    {
        if (transform.position.x > center.position.x - 1.5 && transform.position.x < center.position.x + 1.5 &&
            transform.position.y > center.position.y - 1.5 && transform.position.y < center.position.y + 1.5)
        {
            return true;
        }
        else
            return false;
    }
    /// <summary>
    /// Aims at the player. Must be stopped from outside.
    /// </summary>
    private IEnumerator EnemyAim(Transform target)
    {
        while (true)
        {
            var lookPos = target.position - transform.position;
            lookPos.y = 0; // Only look along xz-plane
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
    public void SetCenter(Transform center)
    {
        this.center = center;
    }
}
