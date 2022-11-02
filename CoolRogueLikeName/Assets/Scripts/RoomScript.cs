using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    float difficulty = 0.5f; // todo
    public Transform player;

    public int pregenerateDepth = 1; // for now, just pregenerate 1 rooms ahead

    private int nEnemies;
    private List<Transform> doors;

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
                // door.gameObject.SendMessage("GenerateRoom", player);
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
            var newRoom = door.gameObject.GetComponent<DoorScript>().GenerateRoom(player);
            newRoom.gameObject.GetComponent<RoomScript>().pregenerateDepth = pregenerateDepth - 1;
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
}
