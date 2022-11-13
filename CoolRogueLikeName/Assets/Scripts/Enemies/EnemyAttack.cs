using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Transform player;
    public float shootRate;   // How often the enemy shoots at the player in seconds.

    /// <summary>
    /// Enemy will shoot periodically in the direction they are facing. 
    /// </summary>
    protected virtual void ShootAtPlayer(Transform spawnPoint, GameObject enemyProjectile, float projectileSpeed, int baseDamage)
    {
        // Create a bullet from the prefab 
        GameObject bullet = Instantiate(enemyProjectile, spawnPoint.position, spawnPoint.rotation);

        // Set projectiles damage
        bullet.GetComponent<Projectile>().Damage = baseDamage;

        // Set its velocity to go forward by projectileSpeed
        bullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * projectileSpeed;
    }

    /// <summary>
    /// Sets the trigger for normal attack
    /// </summary>
    protected virtual void SlashPlayer(Animator swordAnimator)
    {
        swordAnimator.SetTrigger("NormalAttack");
    }
}
