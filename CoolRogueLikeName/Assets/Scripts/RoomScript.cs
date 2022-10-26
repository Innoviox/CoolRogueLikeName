using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    float difficulty = 0.5f; // todo
    public Transform player;

    private int nEnemies;
    private List<Transform> doors;

    // Start is called before the first frame update
    void Start()
    {
        doors = new List<Transform>();

        Transform enemy, door;
        int n = 1;

        while ((enemy = transform.Find($"Enemy{n}")) != null)
        {
            nEnemies++;
            n++;

            enemy.SendMessage("CreateEnemy", player);
        }

        n = 1;
        while ((door = transform.Find($"Door{n}")) != null)
        {
            n++;
            doors.Add(door);
        }

        Debug.Log($"RoomScript: {nEnemies} enemies, {doors.Count} doors");
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
                door.gameObject.SendMessage("GenerateRoom", player);
            }
        }
    }
}
