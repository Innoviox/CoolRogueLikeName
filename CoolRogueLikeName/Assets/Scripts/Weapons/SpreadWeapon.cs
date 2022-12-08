using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadWeapon : BaseWeapon
{
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    // Update is called once per frame
    public override void DoWeaponAction(float damage, float projectileSpeed)
    {
        // Create a bullet from the prefab 
        GameObject bullet = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        GameObject bullet2 = Instantiate(projectile, spawnPoint2.position, spawnPoint2.rotation);
        GameObject bullet3 = Instantiate(projectile, spawnPoint3.position, spawnPoint3.rotation);
        GetComponent<AudioSource>().Play();
        // Set projectiles damage
        bullet.GetComponent<Projectile>().Damage = damage;
        bullet2.GetComponent<Projectile>().Damage = damage;
        bullet3.GetComponent<Projectile>().Damage = damage;

        // Set its velocity to go forward by projectileSpeed
        bullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * projectileSpeed;
        bullet2.GetComponent<Rigidbody>().velocity = spawnPoint2.forward * projectileSpeed;
        bullet3.GetComponent<Rigidbody>().velocity = spawnPoint3.forward * projectileSpeed;
    }
}
