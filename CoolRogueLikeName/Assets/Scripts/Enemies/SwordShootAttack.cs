using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShootAttack : EnemyAttack
{
    private Coroutine attackCo = null; // Store coroutines to stop them at a later time
    private Coroutine moveCo = null;

    public SwordBehaviour swordAttacks;   // Sword behaviour
    public SlashSwordEnemyMovement move;   // inspector for both
    public bool shootingSword;
    private float aimTime;
    public enum swordState 
    {
        attack,
        attacking,
    finishedAttack
    }

    public swordState currentState;

    private void Awake()
    {
        currentState = swordState.attack;
        aimTime = 1.5f;
    }
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
            if (currentState == swordState.attack)
            {
                shootingSword = true;
                StopMoving();
                //Thrust attack 
                // Sword will aim Towards player aimTime seconds
                attackCo = StartCoroutine(swordAttacks.AimTowardsPlayer());
                yield return new WaitForSeconds(aimTime);
                // Both enemy and sword stop aiming at the player
                move.StopAim();
                StopCoroutine(attackCo);
                attackCo = StartCoroutine(swordAttacks.ThrustForward());
                currentState = swordState.attacking;
            }
            else if (currentState == swordState.finishedAttack)
            {
                StartMoving();
                move.StartAim();
                currentState = swordState.attack;
                yield return new WaitForSeconds(2f);
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
