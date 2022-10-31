using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Values/PowerupManager")]
public class PowerupManager : ScriptableObject
{
    public float enemyMoveSpeedFactor;
    public float enemyHealthFactor;
    public float enemyDamageFactor;
    public float playerMoveSpeedFactor;
    public float playerHealthFactor;
    public float playerDamageFactor;
    public float playerReloadSpeedFactor;
    public float bulletSpeedFactor;
    public float playerBulletRangeFactor;
    public float enemySpawnFactor;
    public List<string> acquiredPowerups;

}
