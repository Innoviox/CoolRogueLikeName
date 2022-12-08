using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterShotEnemyAttack : EnemyAttack
{
    // Data for enemy projectile
    public Transform spawnPoint;
    public GameObject enemyProjectile;
    public GameObject breakableEnemyProjectile;
    public float projectileSpeed = 5;
    public int baseDamage = 1;
    public int numBullets;

    protected void Awake()
    {
        shootRate = 1.5f;
        numBullets = 3;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AttackPlayer());
    }

    /// <summary>
    /// Attack the player with a projectile
    /// </summary>
    public IEnumerator AttackPlayer()
    {
        while (true)
        {
            ScatterShotAtPlayer(spawnPoint, breakableEnemyProjectile, enemyProjectile, (a) => (a % 2 == 1), numBullets, projectileSpeed, baseDamage);
            yield return new WaitForSeconds(shootRate);
        }
    }
}
