using System.Collections;
using System.Collections.Generic;
using System;
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
        bullet.GetComponent<EnemyProjectile>().Damage = baseDamage;

        // Set its velocity to go forward by projectileSpeed
        bullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * projectileSpeed;
    }

    /// <summary>
    /// Fires a scattershot at the player made of two types of projectiles. 
    /// You can pass a function to determine which type each bullet will become. 
    /// </summary>
    /// <param name="spawnPoint"></param>
    /// <param name="proj1"></param>
    /// <param name="proj2"></param>
    /// <param name="range"></param>
    /// <param name="numBullets"></param>
    /// <param name="projectileSpeed"></param>
    /// <param name="baseDamage"></param>
    protected virtual void ScatterShotAtPlayer(Transform spawnPoint, GameObject proj1, GameObject proj2, Func<int, bool> range, int numBullets, float projectileSpeed, int baseDamage)
    {
        // Create a bullet from the prefab 
        // GameObject bullet = Instantiate(proj1, spawnPoint.position, spawnPoint.rotation);

        // Set projectiles damage
        // bullet.GetComponent<EnemyProjectile>().Damage = damage1;

        // Set its velocity to go forward by projectileSpeed
        // bullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * projectileSpeed;

        GameObject bullet; // The newly created bullet
        GameObject proj;   // The type of bullet we will create
        Vector3 currDir = spawnPoint.forward;
        float degreeToRotate = 180 / (numBullets + 1); // Used to create arc for bullets spawnd from scatter shot

        // Initial spawn direction for smaller bullets
        currDir = Quaternion.Euler(0, -90 + degreeToRotate, 0) * currDir; // -90 aims directly left of the starting location

        for (int i = 0; i < numBullets; i++)
        {
            // Range to decide which bullets are breakable might pass a delegate
            // proj = (2 <= i && i <= 4) ? proj1 : proj2;
            proj = range(i) ? proj1 : proj2;

            bullet = Instantiate(proj, spawnPoint.position, spawnPoint.rotation);
            bullet.GetComponent<EnemyProjectile>().Damage = baseDamage;
            bullet.GetComponent<Rigidbody>().velocity = currDir * projectileSpeed;

            // Change the direction of the bullet
            currDir = Quaternion.Euler(0, degreeToRotate, 0) * currDir; // 45
        }
    }

    /// <summary>
    /// Sets the trigger for normal attack
    /// </summary>
    protected virtual void SlashPlayer(Animator swordAnimator)
    {
        swordAnimator.SetTrigger("NormalAttack");
    }
}
