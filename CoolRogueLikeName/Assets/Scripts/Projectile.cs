using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage;
    public int collisionCount;
    private LayerMask mask;

    private void Awake()
    {
        mask = LayerMask.GetMask("Wall", "Door", "EnemyBody"); // Floor: unsure which prefab refers to floor.
    }
    // Projectile ends its life when colliding with a wall
    // or an enemy. 
    private void OnCollisionEnter(Collision collision)
    {
        // Don't destroy myself if I collide with other bullets
        if (collision.transform.gameObject.name != "Bullet(Clone)")
        {

            // Destroy myself :(
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (InLayer(other, mask))
        {
            Destroy(gameObject);
        }
    }

    private bool InLayer(Collider other, LayerMask mask)
    {
        return ((1 << other.gameObject.layer) & mask.value) > 0;
    }
}
