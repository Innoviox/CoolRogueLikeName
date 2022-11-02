using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Material doorClosedMaterial;
    public Material doorOpenMaterial;
    public Material doorGoesNowhereMaterial;


    public Transform[] roomPrefabs;

    private Renderer renderer;


    private bool open = false;

    private RoomScript roomThisDoorLeadsTo = null;


    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        renderer.material = doorGoesNowhereMaterial;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!open)
        {
            return;
        }

        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Player")
        {
            // activate enemies
            roomThisDoorLeadsTo.ActivateEnemies();

            // destroy self
            Destroy(gameObject);
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

        return newRoom;
    }

    public void Open()
    {
        if (roomThisDoorLeadsTo != null)
        {
            // set material to open
            renderer.material = doorOpenMaterial;

            open = true;
        }
    }
}
