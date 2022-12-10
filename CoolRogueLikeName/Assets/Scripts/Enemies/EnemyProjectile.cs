using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float Damage;
    public bool breakable; // If this projectile can be destroyed by other projectiles.
    private LayerMask mask;
    private LayerMask maskBreakable;

    private void Awake()
    {
        mask = LayerMask.GetMask("PlayerBody", "Wall", "Door");
        maskBreakable = LayerMask.GetMask("PlayerBody", "Wall", "Door", "PlayerProjectile");
    }
    // Projectile ends its life when colliding with a wall
    // or an enemy. 
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Inside projectile collision");
        string name = collision.transform.gameObject.name;
        // Don't destroy myself if I collide with other bullets
        if (name != "Bullet(Clone)" && name != "EnemyBullet(Clone)")
        {
            // Destroy myself :(
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (breakable && InLayer(other, maskBreakable))
        {
            Destroy(gameObject);
        }
        else if (InLayer(other, mask))
        {
            Destroy(gameObject);
        }
    }

    private bool InLayer(Collider other, LayerMask mask)
    {
        return ((1 << other.gameObject.layer) & mask.value) > 0;
    }

    // If breakable && (name == Bullet || name == wall) -> Destroy
    // else if !breakable && (name == wall) -> Destroy
}
