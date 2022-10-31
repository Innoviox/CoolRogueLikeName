using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float health;
    float maxHealth = 5.0f;

    GameObject healthBar;
    public PowerupManager stats;

    void Start()
    {
        healthBar = transform.parent.Find("HealthBar").gameObject;
        health = maxHealth;
    }

    // Take damage on colliding with projectile
    private void OnCollisionEnter(Collision collision)
    {
        // Check collision occured with a bullet
        // Since bullet is a prefab its gameobject name gets set to Bullet(Clone)
        if (collision.transform.gameObject.name == "Bullet(Clone)")
        {
            // Get the projectiles damage
            float damageTaken = collision.transform.gameObject.GetComponent<Projectile>().Damage * stats.playerDamageFactor;

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
