using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public Transform[] enemyPrefabs;

    void CreateEnemy()
    {
        // create enemY
        var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], new Vector3(0, 0, 0), new Quaternion());
        enemy.parent = transform.parent;
        enemy.localPosition = transform.localPosition;
    }
}
