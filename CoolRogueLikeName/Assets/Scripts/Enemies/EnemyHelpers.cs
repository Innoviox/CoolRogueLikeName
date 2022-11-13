using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyHelpers
{
    /// <summary>
    /// Returns distance from player to the objects position on the x-z plane
    /// </summary>
    public static float DistToPlayer(Transform player, Transform objPos)
    {
        var vectorTarget = player.position - objPos.position;
        // only consider x-z plane distance
        vectorTarget.y = 0;
        return vectorTarget.magnitude;
    }
}
