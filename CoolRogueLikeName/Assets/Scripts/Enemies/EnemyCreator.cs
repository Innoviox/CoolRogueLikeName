using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public Transform[] enemyPrefabs;
    public Transform bossPrefab;

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

    public Transform CreateBoss(Transform player, Vector3 position)
    {
        // create boss
        var boss = Instantiate(bossPrefab, position, new Quaternion());
        boss.parent = transform;
        boss.BroadcastMessage("SetPlayer", player);
        boss.BroadcastMessage("SetCenter", Instantiate(new GameObject(), position, new Quaternion()).transform);

        return boss;
    }

    public void SetOnly(params int[] i)
    {
        var epl = new List<Transform>();
        foreach (int j in i)
        {
            epl.Add(enemyPrefabs[j]);
        }

        enemyPrefabs = epl.ToArray();
    }
}
