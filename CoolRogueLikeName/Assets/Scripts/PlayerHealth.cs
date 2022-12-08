using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public int swordDamage; // Damage received from the enemy sword
    public int waveDamage; // Damage received from the enemy wave attack
    public Transform canvasPrefab;

    GameObject healthBar;
    public float health;
    private Transform canvas;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = transform.Find("HealthBar").gameObject;
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.gameObject.name == "EnemyBullet(Clone)")
        {
            float damageTaken = collision.transform.gameObject.GetComponent<EnemyProjectile>().Damage;
            health -= damageTaken;
        }

        if (collision.transform.gameObject.name == "Sword")
        {
            Debug.Log("Sword Collision");
            health -= swordDamage;
        }

        if (collision.transform.gameObject.name == "BossBullet(Clone)")
        {
            float damageTaken = collision.transform.gameObject.GetComponent<EnemyProjectile>().Damage;
            health -= damageTaken;
        }

        if (collision.transform.gameObject.name == "BossBreakableBullet(Clone)")
        {
            float damageTaken = collision.transform.gameObject.GetComponent<EnemyProjectile>().Damage;
            health -= damageTaken;
        }

        if (collision.transform.gameObject.name == "BossChargeShot(Clone)")
        {
            float damageTaken = collision.transform.gameObject.GetComponent<EnemyProjectile>().Damage;
            health -= damageTaken;
        }

        if (collision.transform.gameObject.name == "WaveBack")
        {
            Debug.Log("Back");
            health -= waveDamage;
        }

        if (collision.transform.gameObject.name == "WaveFront")
        {
            Debug.Log("Front");
            health -= waveDamage;
        }

        if (collision.transform.gameObject.name == "WaveLeft")
        {
            Debug.Log("Left");
            health -= waveDamage;
        }

        if (collision.transform.gameObject.name == "WaveRight")
        {
            Debug.Log("Right");
            health -= waveDamage;
        }

        healthBar.transform.localScale = new Vector3(0.2f, 0.6f * health / maxHealth, 0.2f);
        if (health <= 0.0f)
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
}
