using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnemyAttack : EnemyAttack
{
    private float spawnRate;

    private WaveEnemyMovement move; // Sword enemies movement script 
    public GameObject wave;         // wave attack
    public Transform waveSpawn;     // Wave spawn location

    protected void Awake()
    {
        move = transform.GetComponent<WaveEnemyMovement>();
        spawnRate = 2.5f;
    }

    /// <summary>
    /// spawn a wave towards the player and if the player is too far
    /// then stop attacking and start moving coroutine.
    /// </summary>
    public IEnumerator AttackPlayer()
    {
        float maxDist = 7f;

        while (true)
        {
            if (EnemyHelpers.DistToPlayer(player, transform) < maxDist)
            {
                Instantiate(wave, waveSpawn.position, waveSpawn.rotation);
                yield return new WaitForSeconds(spawnRate);
            }
            else
            {
                StartCoroutine(move.EnemyMove());
                yield break;
            }
        }
    }
}
