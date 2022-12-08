using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Values/PowerupManager")]
public class PowerupManager : ScriptableObject
{
    // used in EnemyMovement
    public float enemyMoveSpeedFactor;

    // used in EnemyScript
    public float enemyHealthFactor;

    // TODO, player doesn't take damage yet
    public float enemyDamageFactor;

    // used in Movement.cs
    public float playerMoveSpeedFactor;

    // TODO, player has no health yet
    public float playerHealthFactor;

    // used in BaseWeapon.cs
    public float playerDamageFactor;

    // used in BaseWeapon.cs
    public float playerReloadSpeedFactor;

    // used in BaseWeapon.cs
    public float bulletSpeedFactor;

    // TODO
    public float playerBulletRangeFactor;

    // TODO
    public float enemySpawnFactor;
    public List<string> acquiredPowerups;

}
