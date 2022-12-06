using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private float health;
    private float maxHealth;
    protected bool inDistance;

    GameObject healthBar;
    public Transform player;

    // Called before Start to initialize 
    private void Awake()
    {
        healthBar = transform.parent.Find("HealthBar").gameObject;
        health = maxHealth = 5.0f;
        inDistance = false;
    }

    void Update()
    {
        // Make the enemy health bar follow the enemy as they move around. 
        healthBar.transform.position = new Vector3(transform.position.x,
                                                   healthBar.transform.position.y,
                                                   transform.position.z);
    }

    /// <summary>
    /// Take damage on colliding with projectile
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        // Check collision occured with a bullet
        // Since bullet is a prefab its gameobject name gets set to Bullet(Clone)
        if (collision.transform.gameObject.name == "Bullet(Clone)")
        {
            // Get the projectiles damage
            float damageTaken = collision.transform.gameObject.GetComponent<Projectile>().Damage;

            health -= damageTaken;

            healthBar.transform.localScale = new Vector3(0.2f, 0.6f * health / maxHealth, 0.2f);
        }

        if (health <= 0.0f)
        {
            Debug.Log("destroyed");
            Debug.Log(transform.parent);
            Debug.Log(transform.parent.parent);
            transform.parent.parent.SendMessage("EnemyDestroyed");
            Destroy(transform.parent.gameObject);
        }
    }
}
