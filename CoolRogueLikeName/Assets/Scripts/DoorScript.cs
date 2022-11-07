using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Material doorClosedMaterial;
    public Material doorUnlockedMaterial;
    public Material doorGoesNowhereMaterial;


    public Transform[] roomPrefabs;

    private Renderer renderer;

    private Collider collider;

    private bool locked = true;

    private bool open = false;

    private RoomScript roomThisDoorLeadsTo = null;


    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        renderer.material = doorGoesNowhereMaterial;

        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

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
            roomThisDoorLeadsTo.ShowRoom(true);
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
            collider.isTrigger = true;

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
        collider.isTrigger = false;
        locked = true;
        renderer.material = doorClosedMaterial;
        renderer.enabled = true;
    }
}
