using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
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


        if (collision.transform.gameObject.name != collideWith.gameObject.name)
        {
            return;
        }

        var player = collideWith.gameObject;


        var newRoom = Instantiate(roomPrefabs[0], new Vector3(0, 0, 0), new Quaternion());

        var entrancePosition = newRoom.Find("Entrance").position;
        var newRoomPosition = transform.position - entrancePosition;

        newRoom.position = newRoomPosition;

        Debug.Log(transform.position + " " + entrancePosition + " " + newRoomPosition);

        Destroy(gameObject);
    }
}
