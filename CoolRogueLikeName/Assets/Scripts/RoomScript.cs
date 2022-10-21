using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    float difficulty = 0.5f; // todo

    private int nEnemies;
    private Transform[] doors;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void EnemyDestroyed()
    {
        nEnemies--;

        if (nEnemies == 0)
        {
            // open all doors
            foreach (var door in doors)
            {
                door.SendMessage("GenerateRoom");
            }
        }
    }
}
