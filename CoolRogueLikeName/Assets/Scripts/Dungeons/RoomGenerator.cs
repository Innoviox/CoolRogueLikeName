using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject floor;
    public GameObject side;
    public GameObject corner;
    public GameObject door;
    public List<GameObject> decorations;
    public float decorationSpawnRate;

    public List<Vector3> sideLocations;

    List<Vector3> doorLocations;

    public void GenerateRoom(Transform roomRootTransform, Vector3 center, int x, int z, List<Door> doorLocations, List<Vector2> locationsNextToDoors)
    {
        this.doorLocations = new List<Vector3>();
        foreach (Door door in doorLocations)
        {
            this.doorLocations.Add(new Vector3(door.x, center.y, door.y));
        }

        for (float i = -x + 1.5f; i < x - 1; i++)
        {
            SideInstantiate(side, new Vector3(center.x + i, center.y, center.z - z - 0.5f + 1.0f), Quaternion.identity, roomRootTransform, locationsNextToDoors);
            SideInstantiate(side, new Vector3(center.x + i, center.y, center.z + z + 0.5f - 1.0f), Quaternion.Euler(0, 180, 0), roomRootTransform, locationsNextToDoors);
            for (float j = -z + 1.5f; j < z - 1; j++)
            {
                Instantiate(floor, new Vector3(center.x + i, center.y, center.z + j), Quaternion.identity, roomRootTransform);
                if (Random.Range(0, 1.0f) < decorationSpawnRate)
                {
                    SideInstantiate(decorations[Random.Range(0, decorations.Count)], new Vector3(center.x + i, center.y, center.z + j), Quaternion.Euler(0, Random.Range(0, 4) * 90, 0), roomRootTransform);
                }
            }
        }
        for (float i = -z + 1.5f; i < z - 1; i++)
        {
            SideInstantiate(side, new Vector3(center.x - x - 0.5f + 1.0f, center.y, center.z + i), Quaternion.Euler(0, 90, 0), roomRootTransform, locationsNextToDoors);
            SideInstantiate(side, new Vector3(center.x + x + 0.5f - 1.0f, center.y, center.z + i), Quaternion.Euler(0, 270, 0), roomRootTransform, locationsNextToDoors);
        }
        Instantiate(corner, new Vector3(center.x + x + 0.5f - 1.0f, center.y, center.z + z + 0.5f - 1.0f), Quaternion.Euler(0, 270, 0), roomRootTransform);
        Instantiate(corner, new Vector3(center.x - x - 0.5f + 1.0f, center.y, center.z + z + 0.5f - 1.0f), Quaternion.Euler(0, 180, 0), roomRootTransform);
        Instantiate(corner, new Vector3(center.x - x - 0.5f + 1.0f, center.y, center.z - z - 0.5f + 1.0f), Quaternion.Euler(0, 90, 0), roomRootTransform);
        Instantiate(corner, new Vector3(center.x + x + 0.5f - 1.0f, center.y, center.z - z - 0.5f + 1.0f), Quaternion.Euler(0, 0, 0), roomRootTransform);

        BoxCollider floorCollider = roomRootTransform.gameObject.AddComponent<BoxCollider>();
        floorCollider.size = new Vector3(2 * x + 0, 1, 2 * z + 0);
        floorCollider.center = new Vector3(0, center.y - 0.5f, 0);

        GameObject triggerHolder = new GameObject("Room " + (int)center.x + " " + (int)center.y + " " + (int)center.z);
        triggerHolder.GetComponent<Transform>().parent = roomRootTransform;
        BoxCollider roomTrigger = triggerHolder.AddComponent<BoxCollider>();
        roomTrigger.size = new Vector3(2 * x - 3, 4, 2 * z - 3);
        roomTrigger.center = new Vector3(center.x, 2, center.z);
        roomTrigger.isTrigger = true;
        TriggerHandler triggerHandler = triggerHolder.AddComponent<TriggerHandler>();
        // return roomRoot;
    }

    void SideInstantiate(GameObject side, Vector3 pos, Quaternion rot, Transform roomRootTransform, List<Vector2> locationsNextToDoors)
    {
        foreach (Vector3 doorPos in doorLocations)
        {
            if (Vector3.Distance(doorPos, pos) <= 1.0f)
            {
                Instantiate(floor, pos, rot, roomRootTransform);
                return;
            }
        }

        foreach (Vector2 doorPos in locationsNextToDoors)
        {
            Vector3 loc = new Vector3(doorPos.x, 0.0f, doorPos.y);
            if (Vector3.Distance(loc, pos) <= 0.5f)
            {
                Instantiate(floor, pos, rot, roomRootTransform);
                return;
            }
        }

        Instantiate(side, pos, rot, roomRootTransform);
    }

    public void Setup()
    {
        sideLocations = new List<Vector3>();
    }
}
