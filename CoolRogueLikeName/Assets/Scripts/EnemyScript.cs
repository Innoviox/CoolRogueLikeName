using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    float health;            
    float maxHealth = 5.0f; 

    GameObject healthBar;
    public Transform player;   

    void Start()
    {
        healthBar = transform.parent.Find("HealthBar").gameObject;
        health = maxHealth;
    }
    
    void Update()
    {
        // The enemy will constantly look at the player.
        transform.LookAt(player);
        // Make the enemy health bar follow the enemy as they move around. 
        healthBar.transform.position = new Vector3(transform.position.x, 
                                                   healthBar.transform.position.y, 
                                                   transform.position.z);
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
            transform.parent.parent.SendMessage("EnemyDestroyed");
            Destroy(transform.parent.gameObject);
        }
    }
}
