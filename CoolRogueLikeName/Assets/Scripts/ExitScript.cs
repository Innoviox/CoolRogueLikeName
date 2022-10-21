using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    public Material doorOpenMaterial;

    public Transform collideWith;
    public Transform[] roomPrefabs;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        // only collide with player
        if (collision.transform.gameObject.name != collideWith.gameObject.name)
        {
            return;
        }

        var player = collideWith.gameObject;

        // destroy self
        Destroy(gameObject);
    }

    private void GenerateRoom()
    {
        // make new room and set its position
        var newRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], new Vector3(0, 0, 0), new Quaternion());

        var entrancePosition = newRoom.Find("Entrance").position;
        var newRoomPosition = transform.position - entrancePosition;

        newRoom.position = newRoomPosition;

        // update the exit script
        // var script = newRoom.Find("Exit").GetComponent<ExitScript>();
        // script.collideWith = collideWith;
        // script.roomPrefabs = roomPrefabs;
    }
}
