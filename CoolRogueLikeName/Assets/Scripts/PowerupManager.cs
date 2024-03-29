using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Values/PowerupManager")]
public class PowerupManager : ScriptableObject
{
    // used in EnemyMovement
    public float enemyMoveSpeedFactor = 1;

    // used in EnemyScript
    public float enemyHealthFactor = 1;

    // used in PlayerHealth
    public float enemyDamageFactor = 1;

    // used in Movement.cs
    public float playerMoveSpeedFactor = 1;

    // used in PlayerHealth
    public float playerHealthFactor = 1;

    // used in BaseWeapon.cs
    public float playerDamageFactor = 1;

    // used in BaseWeapon.cs
    public float playerReloadSpeedFactor = 1;

    // used in BaseWeapon.cs
    public float bulletSpeedFactor = 1;

    // used in Movement.cs
    public int numJumps = 2;
    public int numDashes = 1;
    public float jumpCoolDown = 3.0f;
    public float dashCoolDown = 2.0f;

    // used somewhere
    public float enemySpawnFactor = 0.5f;
    public List<string> acquiredPowerups;

}
