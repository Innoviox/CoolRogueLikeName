using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleGunAttack : EnemyAttack
{
    // Data for enemy projectile
    public Transform spawnPoint;
    public GameObject enemyProjectile;
    public float projectileSpeed = 5;
    public int baseDamage = 1;

    protected void Awake()
    {
        shootRate = 1.5f;
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
            ShootAtPlayer(spawnPoint, enemyProjectile, projectileSpeed, baseDamage);
            yield return new WaitForSeconds(shootRate);
        }
    }
}