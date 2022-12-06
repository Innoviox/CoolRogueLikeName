using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public Transform[] enemyPrefabs;

    public void CreateEnemy(Transform player, Vector3 position)
    {
        // create enemy
        var enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], position, new Quaternion());
        enemy.parent = transform;

        GameObject capsule = enemy.Find("EnemyCapsule").gameObject;
        capsule.GetComponent<EnemyScript>().player = player;
        capsule.GetComponent<EnemyMovement>().player = player;
        capsule.GetComponent<EnemyAttack>().player = player;
    }
}
