using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float Damage;
    public bool breakable; // If this projectile can be destroyed by other projectiles.

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
        //Debug.Log("Inside projectile Trigger");
        string name = other.transform.gameObject.name;
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

        if (other.transform.parent != null && other.transform.parent.name == "WaveAttack(Clone)") { } // Dont destroy when colliding with wave

        else if (breakable && name != "EnemyBreakableBullet(Clone)" && name != "EnemyBullet(Clone)" && name != "BossBreakableBullet(Clone)" && name != "BossBullet(Clone)" && name != "LeftGun" && name != "RightGun" && name != "WaveAttack(Clone)")
        {
            Destroy(gameObject);
        }
        else if (!breakable && name != "EnemyBreakableBullet(Clone)" && name != "Bullet(Clone)" && name != "EnemyBullet(Clone)" && name != "BossBreakableBullet(Clone)" && name != "BossBullet(Clone)" && name != "LeftGun" && name != "RightGun" && name != "WaveAttack(Clone)")
        {
            Destroy(gameObject);
        }
    }

    // If breakable && (name == Bullet || name == wall) -> Destroy
    // else if !breakable && (name == wall) -> Destroy
}
