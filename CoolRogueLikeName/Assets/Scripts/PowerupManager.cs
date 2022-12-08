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

    // TODO, player doesn't take damage yet
    public float enemyDamageFactor = 1;

    // used in Movement.cs
    public float playerMoveSpeedFactor = 1;

    // TODO, player has no health yet
    public float playerHealthFactor = 1;

    // used in BaseWeapon.cs
    public float playerDamageFactor = 1;

    // used in BaseWeapon.cs
    public float playerReloadSpeedFactor = 1;

    // used in BaseWeapon.cs
    public float bulletSpeedFactor = 1;

    // TODO
    public float enemySpawnFactor = 0.5f;
    public List<string> acquiredPowerups;

}
