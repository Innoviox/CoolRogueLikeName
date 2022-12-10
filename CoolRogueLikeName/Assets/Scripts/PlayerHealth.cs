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
    public Transform canvasPrefab;
    public PowerupManager stats;
    public float healAmount;

    GameObject healthBar;
    public float health;
    private Transform canvas;

    private LayerMask enemyProjectilesLayer;
    private LayerMask enemyMeleeLayer;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = transform.Find("HealthBar").gameObject;
        maxHealth = baseMaxHealth * stats.playerHealthFactor;
        health = maxHealth;
        enemyProjectilesLayer = LayerMask.GetMask("EnemyProjectile");
        enemyMeleeLayer = LayerMask.GetMask("EnemyMelee");
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

    private void OnTriggerEnter(Collider other)
    {
        float damageTaken = 0;

        if (InLayer(other, enemyMeleeLayer))
        {
            damageTaken = other.gameObject.GetComponent<EnemySword>().baseDamage;
        } else if (InLayer(other, enemyProjectilesLayer))
        {
            damageTaken = other.gameObject.GetComponent<EnemyProjectile>().Damage;
        }

        if (damageTaken > 0)
        {
            GetComponent<AudioSource>().Play();
            health -= damageTaken * stats.enemyDamageFactor;
        }

        if (healthBar)
        {
            healthBar.transform.localScale = new Vector3(0.2f, 0.6f * health / maxHealth, 0.2f);
        }
        if (health < 0.0f)
        {
            StartCoroutine(DeathScreen());
            // TRIGGER DEATH SCENE
        }
    }

    IEnumerator DeathScreen()
    {
        canvas = Instantiate(canvasPrefab, new Vector3(572.25f, 247.25f, 0), Quaternion.identity);
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(0);
    }

    private bool InLayer(Collider other, LayerMask mask)
    {
        return ((1 << other.gameObject.layer) & mask.value) > 0;
    }
}
