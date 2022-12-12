using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemyAttack : EnemyAttack
{
    private Animator swordAnimator;
    private float slashRate;         // Must be at least the length of animation
                                     // for the enemy to not aimbot as it slashes. 

    private SwordEnemyMovement move; // Sword enemies movement script 

    protected void Awake()
    {
        swordAnimator = transform.Find("Pivot").GetComponent<Animator>();
        move = transform.GetComponent<SwordEnemyMovement>();
        slashRate = 2.5f;
    }

    /// <summary>
    /// Attack the player and if the player is too far
    /// then stop attacking and start moving coroutine.
    /// </summary>
    public IEnumerator AttackPlayer()
    {
        float maxDist = 1.8f;

        while (true)
        {
            if (EnemyHelpers.DistToPlayer(player, transform) < maxDist)
            {
                // Wait until animation is finished to continue
                SlashPlayer(swordAnimator, "NormalAttack");
                yield return new WaitForSeconds(slashRate);
            }
            else
            {
                move.StartAim();
                StartCoroutine(move.EnemyMove());
                yield break;
            }
        }
    }
}
