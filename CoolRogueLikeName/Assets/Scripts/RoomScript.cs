using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    float difficulty = 0.5f; // todo
    public Transform player;

    public int pregenerateDepth = 2; // for now, just pregenerate 1 rooms ahead

    public bool generateEnemies = true; // todo make private with getter/setter

    private int nEnemies;
    private List<Transform> doors;

    private Transform room;
    private Bounds bounds;
    private Bounds playerBounds;
    private DoorScript entryPoint = null;
    private bool roomDone = false;

    // Start is called before the first frame update
    void Start()
    {
        doors = new List<Transform>();

        Transform door;
        int n = 1;

        while ((door = transform.Find($"Door{n}")) != null)
        {
            n++;
            doors.Add(door);
        }

        Debug.Log($"RoomScript: {nEnemies} enemies, {doors.Count} doors");

        PreGenerate();

        if (generateEnemies)
        {
            WalkedInto();
        }

        room = GetComponent<Transform>();
        bounds = room.GetComponent<Collider>().bounds;
        playerBounds = player.GetComponent<Collider>().bounds;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void EnemyDestroyed()
    {
        nEnemies--;
        Debug.Log($"destroyed enemy: {nEnemies} enemies");

        if (nEnemies == 0)
        {
            // finish room
            roomDone = true;

            // open all doors
            foreach (var door in doors)
            {
                // todo make list of DoorScripts so this is faster
                door.gameObject.SendMessage("Unlock");
            }

            if (entryPoint != null)
            {
                entryPoint.Unlock();
            }
        }
    }

    private void PreGenerate()
    {
        if (pregenerateDepth == 0)
        {
            return;
        }

        foreach (var door in doors)
        {
            // todo make this better
            var newRoom = door.gameObject.GetComponent<DoorScript>().GenerateRoom(player);
            newRoom.gameObject.GetComponent<RoomScript>().pregenerateDepth = pregenerateDepth - 1;
            newRoom.gameObject.GetComponent<RoomScript>().generateEnemies = false;
        }
    }

    public void WalkedInto()
    {
        RemoveLid();
        ActivateEnemies();
    }

    private void RemoveLid()
    {
        transform.Find("Ceiling").gameObject.SetActive(false);
    }

    public void ActivateEnemies()
    {
        if (roomDone)
        {
            return;
        }

        Transform enemy;
        int n = 1;

        while ((enemy = transform.Find($"Enemy{n}")) != null)
        {
            nEnemies++;
            n++;

            enemy.SendMessage("CreateEnemy", player);
        }
    }

    public bool PlayerInRoom()
    {
        return bounds.Contains(player.position + playerBounds.min) && bounds.Contains(player.position + playerBounds.max);
    }

    public void AddDoor(Transform door)
    {
        doors.Add(door);
    }

    public void SetEntryPoint(DoorScript door)
    {
        entryPoint = door;
    }

    public bool RoomDone()
    {
        return roomDone;
    }
}
