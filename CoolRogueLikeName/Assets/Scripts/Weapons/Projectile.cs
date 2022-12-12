using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage;
    public int collisionCount;
    protected LayerMask mask;

    private void Awake()
    {
        mask = LayerMask.GetMask("Wall", "Door", "EnemyBody", "EnemyShield"); // Floor: unsure which prefab refers to floor.
    }
    private void OnCollisionEnter(Collision collision)
    {
        DoCollision(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        DoTrigger(other);
    }

    // Projectile ends its life when colliding with a wall
    // or an enemy. 
    protected virtual void DoCollision(Collision collision)
    {
        // Don't destroy myself if I collide with other bullets
        if (collision.transform.gameObject.name != "Bullet(Clone)")
        {
            // Destroy myself :(
            Destroy(gameObject);
        }
    }

    protected virtual void DoTrigger(Collider other)
    {
        if (InLayer(other, mask))
        {
            Destroy(gameObject);
        }
        /*
        if (other.transform.gameObject.name.Contains("Wall") || other.transform.gameObject.name.Contains("Door") || other.transform.gameObject.name.Contains("Floor"))
        {
            Destroy(gameObject);
        }
        */
    }

    protected bool InLayer(Collider other, LayerMask mask)
    {
        return ((1 << other.gameObject.layer) & mask.value) > 0;
    }
}
