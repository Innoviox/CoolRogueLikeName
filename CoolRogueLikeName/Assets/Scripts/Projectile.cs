using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage;
    public int collisionCount;
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

    private void OnTriggerEnter(Collider other) {
        if (other.transform.gameObject.name.Contains("Wall") || other.transform.gameObject.name.Contains("Door") || other.transform.gameObject.name.Contains("Floor")) {
            Destroy(gameObject);
        }
    }
}
