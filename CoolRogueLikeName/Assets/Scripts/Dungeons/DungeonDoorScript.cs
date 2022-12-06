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
    private float opening_speed = 0.1f;
    private int min_y;
    private int max_y;


    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        renderer.material = doorGoesNowhereMaterial;

        collider = GetComponent<Collider>();


        // rotation point is the "bottom left corner" of the door
        rotationPoint = transform.position;
        rotationPoint.y -= transform.localScale.y / 2;

        switch (transform.rotation.eulerAngles.y)
        {
            case 0:
                rotationPoint.x -= transform.localScale.x / 2;
                break;
            case 90:
                rotationPoint.z += transform.localScale.x / 2;
                break;
            case 270:
                rotationPoint.z -= transform.localScale.x / 2;
                break;
            case 360:
                rotationPoint.x += transform.localScale.x / 2;
                break;
        }

        opening_direction = 0;
        min_y = -2;
        max_y = (int)transform.position.y;
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
                roomThisDoorLeadsTo.ShowRoom(true);
                // StartCoroutine(SwingDoor(true, roomThisDoorLeadsTo.PlayerInRoom()));
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
            renderer.material = doorUnlockedMaterial;

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
        renderer.material = doorClosedMaterial;
        renderer.enabled = true;
        opening_direction = 1; // instantly start locking door
    }
}
