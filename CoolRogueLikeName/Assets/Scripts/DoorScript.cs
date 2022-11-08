using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Material doorClosedMaterial;
    public Material doorUnlockedMaterial;
    public Material doorGoesNowhereMaterial;
    public int playerUnlockDistance = 5; // don't know what this should be
    public float maxSwingAngle = 90.0f;
    public Transform[] roomPrefabs;
    public Transform player;
    public RoomScript roomThisDoorLeadsFrom;

    private Renderer renderer;

    private Collider collider;

    private bool locked = true;

    private bool open = false;

    private RoomScript roomThisDoorLeadsTo = null;
    private bool swungOpen = false;
    private Vector3 rotationPoint;


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
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked)
        {
            if (Input.GetKey(KeyCode.E) && collider.bounds.SqrDistance(player.position) < playerUnlockDistance)
            {
                StartCoroutine(SwingDoor(true, roomThisDoorLeadsTo.PlayerInRoom()));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
    }

    private IEnumerator WaitUntilDoorWalkedThrough(bool inRoom)
    {
        // door walked through <=> inRoom status changes
        while (roomThisDoorLeadsTo.PlayerInRoom() == inRoom)
        {
            yield return null;
        }

        DoorWalkedThrough(inRoom);
    }

    private IEnumerator SwingDoor(bool open, bool inRoom)
    {
        if (open == swungOpen)
        {
            yield break; // only run one instance of swing at a time to prevent shenanigans (dancing door)
        }

        swungOpen = open;
        collider.isTrigger = true;

        int mul = open ? -1 : 1;
        mul *= inRoom ? -1 : 1; // if player is in room, swing door the other way

        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            transform.RotateAround(rotationPoint, Vector3.up, mul * maxSwingAngle * Time.deltaTime);
            yield return null;
        }

        collider.isTrigger = false;

        if (open)
        {
            StartCoroutine(WaitUntilDoorWalkedThrough(inRoom));
        }
    }

    void DoorWalkedThrough(bool wasOriginallyinRoom)
    {
        StartCoroutine(SwingDoor(false, wasOriginallyinRoom));


        if (!wasOriginallyinRoom)
        {
            roomThisDoorLeadsTo.WalkedInto();
        }
        else
        {
            roomThisDoorLeadsFrom.WalkedInto();
        }


        if (!roomThisDoorLeadsTo.RoomDone())
        {
            Lock();
        }
    }

    public Transform GenerateRoom(Transform player)
    {
        // make new room and set its position
        var newRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], new Vector3(0, 0, 0), transform.rotation);
        roomThisDoorLeadsTo = newRoom.gameObject.GetComponent<RoomScript>();
        roomThisDoorLeadsTo.player = player;

        var entrancePosition = newRoom.Find("Entrance").position;
        var newRoomPosition = transform.position - entrancePosition;

        newRoom.position = newRoomPosition;

        renderer.material = doorClosedMaterial;

        roomThisDoorLeadsTo.SetEntryPoint(this);

        return newRoom;
    }

    public void Unlock()
    {
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
    }
}
