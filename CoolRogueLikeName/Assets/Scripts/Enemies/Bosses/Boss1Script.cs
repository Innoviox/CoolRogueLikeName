using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1Script : MonoBehaviour
{
    private float chargeAttackTimer; // Cooldown on bosses charge attack
    public float health;             // Bosses health
    private float healthThreshold;   // Decides when the boss will do a wave attack.      
    private int chargeAttackRate;    // How often the boss does a charge attack
    public bool immune;              // Boss is immune to damage during wave attack
    public PowerupManager stats;
    public float baseHealth = 100f;

    private BossAttack attack;       // Bosses attack script
    private BossMovement move;       // Bosses movement script

    private Coroutine attackCo = null; // Store coroutines to stop them at a later time
    private Coroutine moveCo = null;
    public Transform player;

    public bool aimAtPlayer;

    public ScoreManager scoreManager;

    private LayerMask playerProjectileMask;
    private LayerMask playerMeleeMask;
    private Slider bossSlider;

    public enum bossState
    {
        waveAttack,
        waveStart,
        chargeShot,
        normalAttacks,
        wait
    }

    public bossState bState;  // The bosses current state

    private void Awake()
    {
        chargeAttackTimer = Time.time;
        attack = transform.GetComponent<BossAttack>();
        move = transform.GetComponent<BossMovement>();
        health = baseHealth * stats.enemyHealthFactor;
        healthThreshold = health * 0.75f;
        chargeAttackRate = 15;
        aimAtPlayer = true;
        immune = false;

        playerProjectileMask = LayerMask.GetMask("PlayerProjectile");
        playerMeleeMask = LayerMask.GetMask("PlayerMelee");
    }

    void Start()
    {
        StartCoroutine(UpdateState());
        StartCoroutine(bossAI());
    }

    /// <summary>
    /// Dictates what actions the boss will take based on the bosses current state.
    /// </summary>
    /// <returns></returns>
    private IEnumerator bossAI()
    {

        while (true)
        {
            // Wait for the current state to finish executing
            if (bState == bossState.wait) { }
            else if (bState == bossState.waveStart)
            {
                bState = bossState.wait;             // Wait for boss to reach center
                StopNormalAttacks();
                move.StopAim();
                aimAtPlayer = false;
                immune = true;
                StartCoroutine(move.MoveToCenter()); // set waveAttack once center is reached

            }
            else if (bState == bossState.waveAttack)
            {
                bState = bossState.wait;             // Wait for boss to finish executing full wave attack
                StartCoroutine(attack.WaveAttack()); // set normalAttacks once wave is finished
            }
            else if (bState == bossState.chargeShot)
            {
                bState = bossState.wait;
                StopNormalAttacks();
                StartCoroutine(attack.ChargeShot());
            }
            else if (attackCo == null && moveCo == null)
            {
                // Only aim at player if boss is not already aiming at player
                if (move.aimCo == null)
                    move.StartAim(player);

                attackCo = StartCoroutine(attack.NormalAttacks());
                moveCo = StartCoroutine(move.MoveToPlayer());
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// Updates the bosses state throughout the fight. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateState()
    {
        while (true)
        {
            // Wait for boss to finish its current state
            if (bState == bossState.wait) { }
            else if (HpThreshold())
            {
                bState = bossState.waveStart;
            }
            else if (TimerUp())
            {
                chargeAttackTimer = Time.time; // Reset cooldown timer
                bState = bossState.chargeShot;
            }
            else
                bState = bossState.normalAttacks;

            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// Check if cooldown on bosses charge attack is over. 
    /// </summary>
    /// <returns></returns> True if charge attack is up, false otherwise.
    bool TimerUp()
    {
        return (Time.time - chargeAttackTimer > chargeAttackRate);
    }

    /// <summary>
    /// Check if the boss has reached a quarter multiple of their health
    /// </summary>
    /// <returns></returns>
    bool HpThreshold()
    {
        if (health <= healthThreshold)
        {
            healthThreshold -= baseHealth * stats.enemyHealthFactor / 4;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Stop the boss from executing normal attacks and moving.
    /// </summary>
    void StopNormalAttacks()
    {
        if (attackCo != null && moveCo != null)
        {
            StopCoroutine(moveCo);
            StopCoroutine(attackCo);
            moveCo = null;
            attackCo = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check collision occured with a bullet
        if (!immune && InLayer(other, playerProjectileMask))
        {
            TakeDamage(other.transform.gameObject.GetComponent<Projectile>().Damage);
        }
        else if (!immune && InLayer(other, playerMeleeMask))
        {
            TakeDamage(other.transform.gameObject.GetComponent<SwordDamage>().Damage);
        }
    }

    public void TakeDamage(float da)
    {
        health -= da;
        bossSlider.value = health;

        if (health <= 0.0f)
        {
            bossSlider.gameObject.SetActive(false);
            scoreManager.enemyDestroyed();
            transform.parent.parent.SendMessage("EnemyDestroyed");
            Destroy(transform.parent.gameObject);
        }
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }

    private bool InLayer(Collider other, LayerMask mask)
    {
        return ((1 << other.gameObject.layer) & mask.value) > 0;
    }

    public void SetHud(Slider bossSlider)
    {
        this.bossSlider = bossSlider;
        bossSlider.minValue = 0;
        bossSlider.maxValue = health;
        bossSlider.value = health;

        bossSlider.gameObject.SetActive(true);
    }
}
