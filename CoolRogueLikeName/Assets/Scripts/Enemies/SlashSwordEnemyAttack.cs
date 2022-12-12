using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSwordEnemyAttack : EnemyAttack
{
    public Animator flyingSwordAnimator; // Inspector
    private Coroutine moveCo = null;
    public SlashSwordEnemyMovement move;
    public float maxDist;
    public GameObject enemySword;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwordAttack());
    }

    private IEnumerator SwordAttack()
    {
        StartMoving();
        move.StartAim();

        while (true)
        {
            bool tooFar = EnemyHelpers.DistToPlayer(player, transform) > maxDist; // attackDistance has to match dist on movement script 3f

            if (!tooFar)
            {
                StopMoving();
                move.StopAim();
                // Change swords hitbox to be attacking
                enemySword.layer = LayerMask.NameToLayer("EnemyMelee");
                SlashPlayer(flyingSwordAnimator, "FlyingSwordSwing");
                yield return new WaitForSeconds(2.3f); // wait for swing animation
                // change swords hitbox to be defending
                enemySword.layer = LayerMask.NameToLayer("EnemyShield");
                StartMoving();
                move.StartAim();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void StopMoving()
    {
        StopCoroutine(moveCo);
        moveCo = null;
    }

    private void StartMoving()
    {
        moveCo = StartCoroutine(move.EnemyMove());
    }
}
