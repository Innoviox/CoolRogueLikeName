using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoorScript : MonoBehaviour
{
    // the door is a different color based on whether it is locked or unlocked
    public Material doorClosedMaterial;
    public Material doorUnlockedMaterial;
    public Material doorGoesNowhereMaterial;
    public int playerUnlockDistance = 3; // distance at which the door can unlock
    public Transform player; // player
    // rooms this door goes between
    public DungeonRoomScript roomThisDoorLeadsFrom;
    public DungeonRoomScript roomThisDoorLeadsTo;

    private new Renderer renderer;
    private new Collider collider;

    private bool locked = true;
    private bool open = false; // open = "able to be unlocked"
    private int opening_direction; // 0 => none, -1 => down, 1 => up
    private float opening_speed = 0.02f;
    private float min_y;
    public float max_y;
    public Door door;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        collider = GetComponent<Collider>();

        opening_direction = 0;
        min_y = -2.0f;
        max_y = door.isBossDoor ? 0.0f : 0.5f; // boss door is a different size
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked && opening_direction == 0)
        {
            Vector3 position = transform.position;
            position.y = player.position.y;
            if (Vector3.Distance(position, player.position) < playerUnlockDistance)
            {
                roomThisDoorLeadsFrom.ShowRoom(true);
                roomThisDoorLeadsTo.ShowRoom(true);
                opening_direction = -1;
            }
            else
            {
                opening_direction = 1;
            }
        }

        if (opening_direction != 0)
        {
            // move door in opening direction
            Vector3 position = transform.position;
            position.y += opening_direction * opening_speed;

            // clamp position
            if (position.y < min_y)
            {
                position.y = min_y;
                opening_direction = 0;
            }
            else if (position.y > max_y)
            {
                position.y = max_y;
                opening_direction = 0;
            }
            transform.position = position;
        }
    }

    public void Unlock()
    {
        if (renderer == null)
        {
            Start();
        }

        if (roomThisDoorLeadsTo != null)
        {
            // set material to open
            if (!door.isBossDoor)
            {
                renderer.material = doorUnlockedMaterial;
            }
            locked = false;
        }
    }

    public void Lock()
    {
        // lock door
        locked = true;
        if (!door.isBossDoor)
        {
            renderer.material = doorClosedMaterial;
        }
        renderer.enabled = true; // show door
        if (transform.position.y < max_y)
        {
            opening_direction = 1; // instantly start locking door
        }
    }
}
