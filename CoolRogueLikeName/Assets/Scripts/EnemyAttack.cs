using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Holds data on enemy projectile
    public Transform spawnPoint;
    public GameObject enemyProjectile;
    public float projectileSpeed = 5;
    public int baseDamage = 1;

    public float initialWait = 2.0f; // How long to wait before the first call of Invoke in seconds.
    public float shootRate = 1.5f;   // How often the enemy shoots at the player in seconds.

    void Start()
    {
        AttackPlayer();
    }

    public virtual void AttackPlayer()
    {
        InvokeRepeating(nameof(ShootAtPlayer), initialWait, shootRate);
    }

    // Enemy will shoot periodically in the direction they are facing. 
    private void ShootAtPlayer()
    {
        // Create a bullet from the prefab 
        GameObject bullet = Instantiate(enemyProjectile, spawnPoint.position, spawnPoint.rotation);

        // Set projectiles damage
        bullet.GetComponent<Projectile>().Damage = baseDamage;

        // Set its velocity to go forward by projectileSpeed
        bullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * projectileSpeed;
    }
}
