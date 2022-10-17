using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float health;            
    float maxHealth = 5.0f; 

    GameObject healthBar;
    public Transform player;   

    public Transform spawnPoint;     
    public GameObject enemyProjectile;     
    public float projectileSpeed = 5;
    public int baseDamage = 1;

    private float maxDist = 5.0f;   // Distance enemy can be from player before moving. 
    private Rigidbody enemyBody;
    private float enemySpeed = 2;   // Walking speed of enemy
    private int allowedDistance;    // How close enemy can be to an object before changing directions (left/right) to dodge player bullets.
    private bool moveRight;         // Enemy moves right to dodge player bullets by default

    private float shootRate = 1.5f;  // How often the enemy shoots at the player in seconds.
    private float walkRate = 0.1f;   // How often the enemy will walk towards the player based on their distance. 
    public Transform rightSide;      // Used to raycast 
    public Transform leftSide; 


    void Start()
    {
        enemyBody = GetComponent<Rigidbody>();
        healthBar = transform.parent.Find("HealthBar").gameObject;
        health = maxHealth;
        InvokeRepeating(nameof(ShootAtPlayer), 2.0f, shootRate);  
        InvokeRepeating(nameof(WalkTowardsPlayer), 2.0f, walkRate);
        allowedDistance = Random.Range(1, 3);
        moveRight = true;
    }
    
    void Update()
    {
        transform.LookAt(player);
        // Make the enemy health bar follow the enemy as they move around. 
        healthBar.transform.position = new Vector3(transform.position.x, 
                                                   healthBar.transform.position.y, 
                                                   transform.position.z);
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

    // If the player is too far from the enemy on the x-z plane
    // Then the enemy will walk towards the player. 
    private void WalkTowardsPlayer()
    {
        // Get vector between player and enemy position
        var vectorTarget = player.position - transform.position;
        // only consider x-z plane distance
        vectorTarget.y = 0;                       
    
        // Move towards player if player is too far
        if (vectorTarget.magnitude >= maxDist)
        {
            // The enemy walks forward in the direction they are facing
            enemyBody.velocity = new Vector3(transform.forward.x * enemySpeed, 0, transform.forward.z * enemySpeed);
        }
        else
        // Enemy attempts to not be in the line of fire
        {
            // Get direcion player is looking
            Ray playerRay = new Ray(player.position, player.forward);

            RaycastHit hit; // stores what player ray hits

            // Send out a ray from player and store what it hit. 
            if (Physics.Raycast(playerRay, out hit))
            {
                // If the player is looking at me then move based on which direction
                // I have space in.
                if (hit.transform.name == transform.name)
                {
                    // Get distance to objects on my right and left
                    Ray rightOfEnemy = new Ray(rightSide.position, rightSide.right);
                    Ray leftOfEnemy  = new Ray(leftSide.position, leftSide.right * -1);

                    RaycastHit rightHit;
                    RaycastHit leftHit;

                    Physics.Raycast(rightOfEnemy, out rightHit); 
                    Physics.Raycast(leftOfEnemy, out leftHit);

                    // Move to my right
                    if (moveRight)
                    {
                        enemyBody.velocity = enemyBody.transform.right * enemySpeed;
                        if (rightHit.distance < allowedDistance)
                        {
                            moveRight = false;
                            allowedDistance = Random.Range(1, 3);
                        }
                    }
                    // Move to my left
                    else
                    {
                        enemyBody.velocity = enemyBody.transform.right * -1 * enemySpeed;
                        if (leftHit.distance < allowedDistance)
                        {
                            moveRight = true;
                            allowedDistance = Random.Range(1, 3);
                        }
                    }
                }
            }
        }
    }

    // Take damage on colliding with projectile
    private void OnCollisionEnter(Collision collision)
    {
        // Check collision occured with a bullet
        // Since bullet is a prefab its gameobject name gets set to Bullet(Clone)
        if (collision.transform.gameObject.name == "Bullet(Clone)")
        {
            // Get the projectiles damage
            int damageTaken = collision.transform.gameObject.GetComponent<Projectile>().Damage;

            health -= damageTaken;

            healthBar.transform.localScale = new Vector3(0.2f, 0.6f * health / maxHealth, 0.2f);
        }

        if (health <= 0.0f)
        {
            Destroy(gameObject);
            Destroy(healthBar);
        }
    }
}
