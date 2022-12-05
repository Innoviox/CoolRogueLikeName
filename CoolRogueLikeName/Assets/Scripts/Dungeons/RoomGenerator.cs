using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject floor;
    public GameObject side;
    public GameObject corner;
    public GameObject door;

    public void GenerateRoom(Transform roomRootTransform, Vector3 center, int x, int z)
    {
        // GameObject roomRoot = new GameObject("Room " + (int)center.x + " " + (int)center.y + " " + (int)center.z);
        // Transform roomRootTransform = roomRoot.GetComponent<Transform>();
        for (float i = - x + 0.5f; i < x; i++)
        {
            Instantiate(side, new Vector3(center.x + i, center.y, center.z - z - 0.5f), Quaternion.identity, roomRootTransform);
            Instantiate(side, new Vector3(center.x + i, center.y, center.z + z + 0.5f), Quaternion.Euler(0, 180, 0), roomRootTransform);
            for (float j = - z + 0.5f; j < z; j++)
            {
                Instantiate(floor, new Vector3(center.x + i, center.y, center.z + j), Quaternion.identity, roomRootTransform);
            }
        }
        for (float i = - z + 0.5f; i < z; i++)
        {
            Instantiate(side, new Vector3(center.x - x - 0.5f, center.y, center.z + i), Quaternion.Euler(0, 90, 0), roomRootTransform);
            Instantiate(side, new Vector3(center.x + x + 0.5f, center.y, center.z + i), Quaternion.Euler(0, 270, 0), roomRootTransform);
        }
        Instantiate(corner, new Vector3(center.x + x + 0.5f, center.y, center.z + z + 0.5f), Quaternion.Euler(0, 270, 0), roomRootTransform);
        Instantiate(corner, new Vector3(center.x - x - 0.5f, center.y, center.z + z + 0.5f), Quaternion.Euler(0, 180, 0), roomRootTransform);
        Instantiate(corner, new Vector3(center.x - x - 0.5f, center.y, center.z - z - 0.5f), Quaternion.Euler(0, 90, 0), roomRootTransform);
        Instantiate(corner, new Vector3(center.x + x + 0.5f, center.y, center.z - z - 0.5f), Quaternion.Euler(0, 0, 0), roomRootTransform);

        BoxCollider floorCollider = roomRootTransform.gameObject.AddComponent<BoxCollider>();
        floorCollider.size = new Vector3(2 * x + 2, 1, 2 * z + 2);
        floorCollider.center = new Vector3(center.x, center.y - 0.5f, center.z);

        // return roomRoot;
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown("a"))
    //     {
    //         GenerateRoom(new Vector3(0f, 0f, 0f), 5, 5);
    //     }
    // }
}
