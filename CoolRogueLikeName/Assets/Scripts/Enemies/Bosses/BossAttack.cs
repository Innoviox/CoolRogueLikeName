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
    private float degreeToRotate;    // Used to create arc for bullets spawnd from scatter shot
    private int numWaves;

    private void Awake()
    {
        numBullets = 8;
        boss = transform.GetComponent<Boss1Script>();
        move = transform.GetComponent<BossMovement>();
        shootRate = 1.5f;
        numWaves = 4;
        numNormalShots = 3;

        degreeToRotate = 180 / (numBullets + 1);
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

            // fires a scattershot at the player
            // ScatterShot();

            // Half of the bullets are breakable
            int start = 0 + (numBullets / 4);
            int end = numBullets - (numBullets / 4) - 1;

            ScatterShotAtPlayer(rightSpawnPoint, breakableEnemyProjectile, enemyProjectile, (a) => (a >= start && a <= end), numBullets, projectileSpeed, baseDamage);
            yield return new WaitForSeconds(shootRate);
        }
    }

    /// <summary>
    /// Uses the right bosses gun to fire a scatter shot. Bullets are either
    /// breakable or not breakable. 
    /// </summary>
    void ScatterShot()
    {
        GameObject bullet; // The newly created bullet
        GameObject proj;   // The type of bullet we will create
        Vector3 currDir = rightSpawnPoint.forward;

        // Initial spawn direction for smaller bullets
        currDir = Quaternion.Euler(0, -90 + degreeToRotate, 0) * currDir; // -90 aims directly left of the starting location

        for (int i = 0; i < numBullets; i++)
        {
            // Range to decide which bullets are breakable
            proj = (2 <= i && i <= 4) ? breakableEnemyProjectile : enemyProjectile;

            bullet = Instantiate(proj, rightSpawnPoint.position, rightSpawnPoint.rotation);
            bullet.GetComponent<EnemyProjectile>().Damage = baseDamage;
            bullet.GetComponent<Rigidbody>().velocity = currDir * projectileSpeed;

            // Change the direction of the bullet
            currDir = Quaternion.Euler(0, degreeToRotate, 0) * currDir; // 45
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

        // Set projectiles damage
        bullet.GetComponent<BossChargeShot>().Damage = 15;

        // Set its velocity to go forward by projectileSpeed
        bullet.GetComponent<Rigidbody>().velocity = dir * projectileSpeed;

        float timer = Time.time;
        // Wait three seconds after shooting charge shot to continue
        while (timer + 3 >= Time.time)
        {
            yield return new WaitForSeconds(1f);
        }

        boss.bState = Boss1Script.bossState.normalAttacks;
        yield return null;
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
        yield return null;
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
        yield return null;
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
}
