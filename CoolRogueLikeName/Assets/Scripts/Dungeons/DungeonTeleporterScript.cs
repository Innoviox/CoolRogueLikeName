using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTeleporterScript : MonoBehaviour
{
    public delegate void TeleportDelegate();
    public TeleportDelegate teleport;
    public Transform player;
    public ScoreManager scoreManager;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < 2)
        {
            scoreManager.levelCleared();
            teleport();
        }
    }
}
