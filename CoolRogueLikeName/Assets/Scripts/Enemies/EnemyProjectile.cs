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
        //Debug.Log("Mask value is : " + mask.value);
        //Debug.Log("Maskbreakable value is : " + maskBreakable.value);

        //mask = LayerMask.GetMask("PlayerBody");
        //Debug.Log("Mask is: " + " Mask value is : "  + mask.value);
        // other.gameObject.layer;
        //Debug.Log("Inside projectile Trigger");
        // string name = other.transform.gameObject.name;
        // Don't destroy myself if I collide with other bullets
        //if (name != "Bullet(Clone)" && name != "EnemyBullet(Clone)")
        //{
        // Destroy myself :(
        //  Destroy(gameObject);
        // }
        // Need a better way to do this, layermask? 

        // Player Bullet: destroyed when making contact with enemy or wall
        // Enemy Bullet: Breakable, destroyed when making contact with player bullet or walls
        //               Non-breakable, destroyed when making contact with player or wall
        //Debug.Log("Inside projectile Trigger, I am : " + gameObject.transform.name);
        //Debug.Log("The layer i'm colliding is : " + other.gameObject.layer);
        if (breakable && InLayer(other, maskBreakable))
        {
            Destroy(gameObject);
        }
        else if (InLayer(other, mask))
        {
            Destroy(gameObject);
        }
        /*
        if (other.transform.parent != null && other.transform.parent.name == "WaveAttack(Clone)") { } // Dont destroy when colliding with wave

        else if (breakable && name != "EnemyBreakableBullet(Clone)" && name != "EnemyBullet(Clone)" && name != "BossBreakableBullet(Clone)" && name != "BossBullet(Clone)" && name != "LeftGun" && name != "RightGun" && name != "WaveAttack(Clone)")
        {
            Destroy(gameObject);
        }
        else if (!breakable && name != "EnemyBreakableBullet(Clone)" && name != "Bullet(Clone)" && name != "EnemyBullet(Clone)" && name != "BossBreakableBullet(Clone)" && name != "BossBullet(Clone)" && name != "LeftGun" && name != "RightGun" && name != "WaveAttack(Clone)")
        {
            Destroy(gameObject);
        }
        */
    }

    private bool InLayer(Collider other, LayerMask mask)
    {
        return ((1 << other.gameObject.layer) & mask.value) > 0;
    }

    // If breakable && (name == Bullet || name == wall) -> Destroy
    // else if !breakable && (name == wall) -> Destroy
}
