using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public Transform spawnPoint;   // Select this through the inspector
    public GameObject projectile;  // Selected Bullet prefab through inspector
    public float baseProjectileSpeed = 5;
    public float baseCooldown = 0.25f;
    public int baseDamage = 1;
    public PowerupManager stats;
    public string firebutton;

    public void EnableWeapon()
    {
        StartCoroutine(WeaponCoroutine());
    }
    public void DisableWeapon()
    {
        StopCoroutine(WeaponCoroutine());
    }

    IEnumerator WeaponCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetButtonDown(firebutton));
            DoWeaponAction(baseDamage * stats.playerDamageFactor, baseProjectileSpeed * stats.bulletSpeedFactor);
            yield return new WaitForSeconds(baseCooldown * stats.playerReloadSpeedFactor);
        }
    }

    // Don't do any powerup scaling, it has already been done
    public virtual void DoWeaponAction(float damage, float projectileSpeed)
    {
        // Create a bullet from the prefab 
        GameObject bullet = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        GetComponent<AudioSource>().Play();
        // Set projectiles damage
        bullet.GetComponent<Projectile>().Damage = baseDamage;

        // Set its velocity to go forward by projectileSpeed
        bullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * projectileSpeed;
    }

}
