using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSwordEnemyMovement : EnemyMovement
{
    public Coroutine aimCo = null;
    public float maxDist;

    private void Awake()
    {
        enemyBody = GetComponent<Rigidbody>();
        walkRate = 0.02f;
    }

    /// <summary>
    /// Move towards the player and once close enough stop moving
    /// and start attacking coroutine.
    /// </summary>
    public IEnumerator EnemyMove()
    {

        while (true)
        {
            if (EnemyHelpers.DistToPlayer(player, transform) > maxDist)
            {
                WalkTowardsPlayer(maxDist);
            }
            else
                enemyBody.velocity = new Vector3(0, 0, 0);

            yield return new WaitForSeconds(walkRate);
        }
    }

    /// <summary>
    /// Starts the enemies aim coroutine
    /// </summary>
    public void StartAim()
    {
        aimCo = StartCoroutine(EnemyAim());
    }

    /// <summary>
    /// Stops enemies aim coroutine
    /// </summary>
    public void StopAim()
    {
        StopCoroutine(aimCo);
        aimCo = null;
    }

    private IEnumerator EnemyAim()
    {
        while (true)
        {
            var lookPos = player.position - transform.position;
            lookPos.y = 0; // Only look along xz-plane
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);

            yield return new WaitForSeconds(0.01f);
        }
    }
}
