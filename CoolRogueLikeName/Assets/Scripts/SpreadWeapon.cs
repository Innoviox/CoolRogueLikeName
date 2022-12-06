using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadWeapon : MonoBehaviour
{

    public Transform spawnPoint1;   // Select this through the inspector
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public GameObject projectile;  // Selected Bullet prefab through inspector
    public float projectileSpeed = 5;
    public int baseDamage = 1;
    public float cooldown = 0.5F;
    private float nextAttack;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > nextAttack)  //  Comment this line for scatter
        {   
            nextAttack = Time.time + cooldown;
            // Create a bullet from the prefab 
            GameObject bullet = Instantiate(projectile, spawnPoint1.position, spawnPoint1.rotation);
            GameObject bullet2 = Instantiate(projectile, spawnPoint2.position, spawnPoint2.rotation);
            GameObject bullet3 = Instantiate(projectile, spawnPoint3.position, spawnPoint3.rotation);
            // Set projectiles damage
            bullet.GetComponent<Projectile>().Damage = baseDamage;
            bullet2.GetComponent<Projectile>().Damage = baseDamage;
            bullet3.GetComponent<Projectile>().Damage = baseDamage;

            // Set its velocity to go forward by projectileSpeed
            bullet.GetComponent<Rigidbody>().velocity = spawnPoint1.forward * projectileSpeed;
            bullet2.GetComponent<Rigidbody>().velocity = spawnPoint2.forward * projectileSpeed;
            bullet3.GetComponent<Rigidbody>().velocity = spawnPoint3.forward * projectileSpeed;
        }
    }
}
