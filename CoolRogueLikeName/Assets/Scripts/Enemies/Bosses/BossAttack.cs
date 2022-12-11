using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BossAttack : EnemyAttack
{
    public GameObject enemyProjectile;
    public GameObject breakableEnemyProjectile;
    public GameObject chargeProjectile;
    public GameObject wave;                  // wave attack
    public Transform waveSpawn;              // Wave spawn location
    public Transform rightSpawnPoint;        // Right gun spawn point
    public Transform leftSpawnPoint;         // Left gun spawn point

    private Boss1Script boss;
    private BossMovement move;

    private int numBullets;
    public int baseDamage = 1;
    public float projectileSpeed = 5;
    private int numNormalShots;
    private int numWaves;

    private void Awake()
    {
        numBullets = 8;
        boss = transform.GetComponent<Boss1Script>();
        move = transform.GetComponent<BossMovement>();
        shootRate = 2.5f;
        numWaves = 4;
        numNormalShots = 3;

    }

    /// <summary>
    /// Shoots at the player cycling between normal shots and scatter shots 
    /// based on the ratio numNormalShots for every 1 scatter. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator NormalAttacks()
    {
        while (true)
        {
            // Shoots standard projectiles at the player
            for (int i = 0; i < numNormalShots; i++)
            {
                ShootAtPlayer(leftSpawnPoint, enemyProjectile, projectileSpeed, baseDamage);
                yield return new WaitForSeconds(shootRate);
            }

            // Half of the bullets are breakable
            int start = 0 + (numBullets / 4);
            int end = numBullets - (numBullets / 4) - 1;

            ScatterShotAtPlayer(rightSpawnPoint, breakableEnemyProjectile, enemyProjectile, (a) => (a >= start && a <= end), numBullets, projectileSpeed, baseDamage);
            yield return new WaitForSeconds(shootRate);
        }
    }

    /// <summary>
    /// Create a Charge Shot coming from the bosses right gun. 
    /// Wait for some time after a charge is fired to continue attacking the player. 
    /// </summary>
    /// <returns></returns>
    public IEnumerator ChargeShot()
    {
        // Force the charge shot to travel parallel to the ground 
        Vector3 pos = rightSpawnPoint.position;
        Vector3 dir = rightSpawnPoint.forward;
        pos.y = 0.5f;
        dir.y = 0;
        // chargeProjectiles entry animation is played when instantiated which scales its size. 
        GameObject bullet = Instantiate(chargeProjectile, pos, rightSpawnPoint.rotation);

        // Set its velocity to go forward by projectileSpeed
        bullet.GetComponent<Rigidbody>().velocity = dir * projectileSpeed;

        yield return new WaitForSeconds(3f);
        boss.bState = Boss1Script.bossState.normalAttacks;
    }

    // WaveAttack
    public IEnumerator WaveAttack()
    {
        StartCoroutine(RotateObject(45, Vector3.up, 1)); // rotate 45 degrees around y every second
        float timer;

        for (int i = 0; i < numWaves; i++)
        {
            Instantiate(wave, waveSpawn.position, waveSpawn.rotation);

            timer = Time.time;
            // Wait four seconds after making wave to continue
            while (timer + 2 >= Time.time)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        move.StartAim(player);
        boss.bState = Boss1Script.bossState.normalAttacks;
        boss.aimAtPlayer = true;
        boss.immune = false;
    }

    IEnumerator RotateObject(float angle, Vector3 axis, float inTime)
    {
        // calculate rotation speed
        float rotationSpeed = angle / inTime;
        int counter = 0;

        // Four rotations are executed
        while (counter < 4)
        {
            // save starting rotation position
            Quaternion startRotation = transform.rotation;

            float deltaAngle = 0;

            // rotate until reaching angle
            while (deltaAngle < angle)
            {
                deltaAngle += rotationSpeed * Time.deltaTime;
                deltaAngle = Mathf.Min(deltaAngle, angle);

                transform.rotation = startRotation * Quaternion.AngleAxis(deltaAngle, axis);

                yield return null;
            }
            counter++;
            // delay here
            yield return new WaitForSeconds(1);
        }
    }
}
