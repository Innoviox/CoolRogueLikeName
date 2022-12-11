using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public PowerupManager stats;
    float health;
    float maxHealth = 5.0f;

    GameObject healthBar;
    public Transform player;
    private LayerMask playerTriggerProjectiles;
    private LayerMask playerTriggerMelee;
    public GameObject deathSoundContainer;

    public ScoreManager scoreManager;
    public bool dead = false;

    void Start()
    {
        healthBar = transform.parent.Find("HealthBar").gameObject;
        health = maxHealth;
        playerTriggerProjectiles = LayerMask.GetMask("PlayerProjectile");
        playerTriggerMelee = LayerMask.GetMask("PlayerMelee");
        dead = false;
    }

    void Update()
    {
        // Make the enemy health bar follow the enemy as they move around. 
        healthBar.transform.position = new Vector3(transform.position.x,
                                                   healthBar.transform.position.y,
                                                   transform.position.z);
    }

    // Take damage on colliding with projectile
    private void OnCollisionEnter(Collision collision)
    {
        if (InLayer(collision.collider, playerTriggerMelee))
        {
            takeDamage(collision.transform.gameObject.GetComponent<SwordDamage>().Damage);
        }
        else if (InLayer(collision.collider, playerTriggerProjectiles))
        {
            takeDamage(collision.transform.gameObject.GetComponent<Projectile>().Damage);
        }
    }

    // take damage on being in sword hitbox
    private void OnTriggerEnter(Collider collision)
    {
        if (InLayer(collision, playerTriggerMelee))
        {
            takeDamage(collision.transform.gameObject.GetComponent<SwordDamage>().Damage);
        }
        else if (InLayer(collision, playerTriggerProjectiles))
        {
            takeDamage(collision.transform.gameObject.GetComponent<Projectile>().Damage);
        }

    }

    private bool InLayer(Collider other, LayerMask mask)
    {
        return ((1 << other.gameObject.layer) & mask.value) > 0;
    }

    private void takeDamage(float da)
    {
        health -= da;

        healthBar.transform.localScale = new Vector3(0.2f, 0.6f * health / maxHealth, 0.2f);

        if (!dead && health <= 0.0f)
        {
            scoreManager.enemyDestroyed();
            transform.parent.parent.SendMessage("EnemyDestroyed");
            GameObject deathSound = Instantiate(deathSoundContainer, transform.position, Quaternion.identity);
            Destroy(deathSound, deathSoundContainer.GetComponent<AudioSource>().clip.length);
            Destroy(transform.parent.gameObject);
            dead = true;
        }
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
}
