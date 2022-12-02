using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTeleporterScript : MonoBehaviour
{
    public delegate void TeleportDelegate();
    public TeleportDelegate teleport;
    public Transform player;
    // todo animate?

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(transform.position, player.position));
        if (Vector3.Distance(transform.position, player.position) < 2)
        {
            teleport();
        }
    }
}
