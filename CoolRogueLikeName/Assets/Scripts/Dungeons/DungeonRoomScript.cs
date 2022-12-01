using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomScript : MonoBehaviour
{
    public Room room;
    public Transform player;
    private Bounds bounds;
    private Bounds playerBounds;

    // Start is called before the first frame update
    void Start()
    {
        bounds = room.GetBounds();
        playerBounds = player.GetComponent<Collider>().bounds;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
