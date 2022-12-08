using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public Transform spawnPoint;   // Select this through the inspector
    public GameObject projectile;  // Selected Bullet prefab through inspector
    public float projectileSpeed = 5;
    public int baseDamage = 1;


    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButton("Fire1"))         // Uncomment me for scatter shot 
        if (Input.GetKeyDown(KeyCode.Mouse0))  //  Comment this line for scatter
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
}
