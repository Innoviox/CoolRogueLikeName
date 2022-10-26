using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public Transform[] enemyPrefabs;

    void CreateEnemy(Transform player)
    {
        // create enemy
        var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], new Vector3(0, 0, 0), new Quaternion());
        enemy.parent = transform.parent;
        enemy.localPosition = transform.localPosition;

        GameObject capsule = enemy.Find("EnemyCapsule").gameObject;
        capsule.GetComponent<EnemyScript>().player = player;
        capsule.GetComponent<EnemyMovement>().player = player;
    }
}
