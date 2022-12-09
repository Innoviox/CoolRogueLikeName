using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float baseMaxHealth;
    public float maxHealth;
    public int swordDamage; // Damage received from the enemy sword
    public int waveDamage; // Damage received from the enemy wave attack
    public PowerupManager stats;
    public float healAmount;

    GameObject healthBar;
    public float health;

    public ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = transform.Find("HealthBar").gameObject;
        maxHealth = baseMaxHealth * stats.playerHealthFactor;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // This makes the health bar follow the player
        healthBar.transform.position = new Vector3(transform.position.x,
                                                   transform.position.y + 1,
                                                   transform.position.z);
    }

    public void UpdateMaxHealth()
    {
        float ratio = health / maxHealth;
        maxHealth = baseMaxHealth * stats.playerHealthFactor;
        health = maxHealth * ratio;
        healthBar.transform.localScale = new Vector3(0.2f, 0.6f * health / maxHealth, 0.2f);

    }

    public void Heal(int count)
    {
        health += healAmount * count;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.transform.localScale = new Vector3(0.2f, 0.6f * health / maxHealth, 0.2f);

    }

    private void OnTriggerEnter(Collider collision)
    {
        float damageTaken = 0;

        if (collision.transform.gameObject.name == "EnemyBullet(Clone)")
        {
            damageTaken = collision.transform.gameObject.GetComponent<EnemyProjectile>().Damage;
        }

        if (collision.transform.gameObject.name == "Sword")
        {
            damageTaken = swordDamage;
        }

        if (collision.transform.gameObject.name == "BossBullet(Clone)")
        {
            damageTaken = collision.transform.gameObject.GetComponent<EnemyProjectile>().Damage;
        }

        if (collision.transform.gameObject.name == "BossBreakableBullet(Clone)")
        {
            damageTaken = collision.transform.gameObject.GetComponent<EnemyProjectile>().Damage;
        }

        if (collision.transform.gameObject.name == "BossChargeShot(Clone)")
        {
            damageTaken = collision.transform.gameObject.GetComponent<EnemyProjectile>().Damage;
        }

        if (collision.transform.gameObject.name == "WaveBack" ||
            collision.transform.gameObject.name == "WaveFront" ||
            collision.transform.gameObject.name == "WaveLeft" ||
            collision.transform.gameObject.name == "WaveRight")
        {
            damageTaken = waveDamage;
        }

        health -= damageTaken * stats.enemyDamageFactor;

        Debug.Log(maxHealth);
        healthBar.transform.localScale = new Vector3(0.2f, 0.6f * health / maxHealth, 0.2f);
        if (health <= 0.0f)
        {
            Debug.Log("Player Died");
            scoreManager.playerDeath();
            SceneManager.LoadScene(2);
        }
    }
}
