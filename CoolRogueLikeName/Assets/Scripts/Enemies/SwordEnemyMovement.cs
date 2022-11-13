using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemyMovement : EnemyMovement
{
    private SwordEnemyAttack attack; // Sword enemies attack script
    public IEnumerator aimCo;        // Used to call Stopcoroutine on

    protected void Awake()
    {
        attack = transform.GetComponent<SwordEnemyAttack>();
        enemyBody = GetComponent<Rigidbody>();
        allowedDistance = RandomDistance();
        moveRight = true;
        walkRate = 0.2f;
        aimCo = EnemyAim();
    }

    protected void Start()
    {
        StartCoroutine(aimCo);
        StartCoroutine(EnemyMove());
    }

    /// <summary>
    /// Move towards the player and once close enough stop moving
    /// and start attacking coroutine.
    /// </summary>
    public IEnumerator EnemyMove()
    {
        bool tooFar = true;
        while (true)
        {
            float maxDist = 1.8f;

            if (tooFar)
            {
                tooFar = WalkTowardsPlayer(maxDist);
                yield return new WaitForSeconds(walkRate);
            }
            else
            {
                StopCoroutine(aimCo);
                StartCoroutine(attack.AttackPlayer());
                yield break;
            }
        }
    }

    /// <summary>
    /// Starts the enemies aim coroutine
    /// </summary>
    public void StartAim()
    {
        StartCoroutine(aimCo);
    }

    /// <summary>
    /// Aims at the player. Must be stopped from outside.
    /// </summary>
    private IEnumerator EnemyAim()
    {
        while (true)
        {
            transform.LookAt(player);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
