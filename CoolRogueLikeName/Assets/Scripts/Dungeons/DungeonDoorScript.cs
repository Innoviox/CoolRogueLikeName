using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonDoorScript : MonoBehaviour
{
    public Material doorClosedMaterial;
    public Material doorUnlockedMaterial;
    public Material doorGoesNowhereMaterial;
    public int playerUnlockDistance = 5; // don't know what this should be
    public float maxSwingAngle = 90.0f;
    public Transform[] roomPrefabs;
    public Transform player;
    public DungeonRoomScript roomThisDoorLeadsFrom;

    private new Renderer renderer;

    private new Collider collider;

    private bool locked = true;

    private bool open = false;

    public DungeonRoomScript roomThisDoorLeadsTo = null;
    private bool swungOpen = false;
    private Vector3 rotationPoint;

    private int opening_direction; // 0 => none, -1 => down, 1 => up
    private float opening_speed = 0.02f;
    private float min_y;
    public float max_y;
    public Door door;


    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        // renderer.material = doorGoesNowhereMaterial;

        collider = GetComponent<Collider>();

        opening_direction = 0;
        min_y = -2.0f;
        max_y = door.isBossDoor ? 0.0f : 0.5f;
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
            Vector3 position = transform.position;
            position.y += opening_direction * opening_speed;
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

    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    public void Unlock()
    {
        if (renderer == null)
        {
            Start(); // i literally have no idea how this method could get called before start but it does sometimes so here we are
        }

        if (roomThisDoorLeadsTo != null)
        {
            // set material to open
            if (!door.isBossDoor)
            {
                renderer.material = doorUnlockedMaterial;
            }

            // set collider to trigger
            // collider.isTrigger = true;
            locked = false;
        }
    }

    public void Open()
    {
        renderer.enabled = false;
    }

    public void Lock()
    {
        // lock door
        // collider.isTrigger = false;
        locked = true;
        if (!door.isBossDoor)
            renderer.material = doorClosedMaterial;
        renderer.enabled = true;
        if (transform.position.y < max_y)
            opening_direction = 1; // instantly start locking door
    }
}
