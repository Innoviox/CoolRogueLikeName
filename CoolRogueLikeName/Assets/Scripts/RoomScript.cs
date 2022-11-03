using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    float difficulty = 0.5f; // todo
    public Transform player;

    public int pregenerateDepth = 1; // for now, just pregenerate 1 rooms ahead

    public bool generateEnemies = true; // todo make private with getter/setter

    private int nEnemies;
    private List<Transform> doors;

    private Transform room;

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
            ActivateEnemies();
        }

        room = GetComponent<Transform>();
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
            // open all doors
            foreach (var door in doors)
            {
                // for now dont do this
                // todo make list of DoorScripts so this is faster
                door.gameObject.SendMessage("Open");
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

    public void ActivateEnemies()
    {
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
        var bounds = room.GetComponent<Collider>().bounds;
        var playerBounds = player.GetComponent<Collider>().bounds;
        return bounds.Contains(playerBounds.min) && bounds.Contains(playerBounds.max);
    }
}
