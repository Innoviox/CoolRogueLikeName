using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Material doorClosedMaterial;
    public Material doorUnlockedMaterial;
    public Material doorGoesNowhereMaterial;
    public int playerUnlockDistance = 5; // don't know what this should be

    public Transform[] roomPrefabs;
    public Transform player;

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

        Debug.Log($"DoorScript: {transform.rotation.eulerAngles.y} {rotationPoint}");
    }

    // Update is called once per frame
    void Update()
    {
        if (!locked)
        {
            if (Input.GetKey(KeyCode.E) && collider.bounds.SqrDistance(player.position) < playerUnlockDistance)
            {
                StartCoroutine(SwingDoorOpen());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if (locked)
        // {
        //     return;
        // }

        // Debug.Log(collision.gameObject.name);
        // if (collision.gameObject.name == "Player")
        // {
        //     renderer.enabled = false;
        //     StartCoroutine(WaitUntilDoorWalkedThrough());
        // }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (locked)
        {
            return;
        }

        if (other.gameObject.name == "Player")
        {
            Open();
            StartCoroutine(WaitUntilDoorWalkedThrough());
        }
    }

    private IEnumerator WaitUntilDoorWalkedThrough()
    {
        while (!roomThisDoorLeadsTo.PlayerInRoom())
        {
            yield return null;
        }

        DoorWalkedThrough();
    }

    private IEnumerator SwingDoorOpen()
    {
        if (swungOpen)
        {
            yield break; // only run one instance of swing at a time to prevent shenanigans (dancing door)
        }

        swungOpen = true;

        int rotation = 0;
        while (rotation < 360)
        {
            transform.RotateAround(rotationPoint, Vector3.up, -90 * Time.deltaTime);
            rotation++;
            yield return null;
        }
    }

    void DoorWalkedThrough()
    {
        if (roomThisDoorLeadsTo.RoomDone())
        {
            return;
        }

        // activate enemies
        roomThisDoorLeadsTo.WalkedInto();

        Lock();
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
