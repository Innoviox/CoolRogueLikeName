using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    public Transform spawnPoint;   // Select this through the inspector
    public GameObject projectile;  // Selected Bullet prefab through inspector
    public float projectileSpeed = 8;
    public int baseDamage = 1;
    
    public float cooldown = 0.2F;
    private float nextAttack;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > nextAttack)  //  Comment this line for scatter
        {   
            nextAttack = Time.time + cooldown;
            // Create a bullet from the prefab 
            GameObject bullet = Instantiate(projectile, spawnPoint.position, projectile.transform.rotation);

            // Set projectiles damage
            bullet.GetComponent<RocketProjectile>().Damage = baseDamage;
            

            // Set its velocity to go forward by projectileSpeed
            bullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * projectileSpeed;
    }
    }
}