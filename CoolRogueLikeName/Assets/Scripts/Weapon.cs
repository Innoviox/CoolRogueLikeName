using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform spawnPoint;   // Select this through the inspector
    public GameObject projectile;  // Selected Bullet prefab through inspector
    public float baseProjectileSpeed = 5;
    public float baseDamage = 1;
    public float baseCooldown = 0.5f;
    public PowerupManager stats;

    protected bool readyToFire = true;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (readyToFire)
        {
            //if (Input.GetButton("Fire1"))         // Uncomment me for scatter shot 
            if (Input.GetKeyDown(KeyCode.Mouse0))  //  Comment this line for scatter
            {
                // Create a bullet from the prefab 
                GameObject bullet = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);

                // Set projectiles damage
                bullet.GetComponent<Projectile>().Damage = baseDamage * stats.playerDamageFactor * stats.playerDamageFactor;

                // Set its velocity to go forward by projectileSpeed
                bullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * baseProjectileSpeed * stats.bulletSpeedFactor;

                readyToFire = false;

                // do cooldown
                StartCoroutine(Cooldown());
            }
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(baseCooldown* stats.playerReloadSpeedFactor);
        readyToFire = true;
    }
}
