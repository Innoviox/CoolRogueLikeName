using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectile : Projectile
{
    //public int Explosion; // was 1
    public GameObject explosionPrefab;
    public float explosionTime;
    private GameObject explosion;
    // Projectile ends its life when colliding with a wall
    // or an enemy. 
    // Spawns explosion radius and slight VFX to convey an explosion.
    protected override void DoCollision(Collision collision)
    {
        // Don't destroy myself if I collide with other bullets
        if (collision.transform.gameObject.name != "RocketProj(Clone)")
        {
            explosion = Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
            // Destroy myself :(
            Destroy(gameObject);

            Destroy(explosion, explosion.GetComponent<AudioSource>().clip.length-0.2F);
        }

    }

    protected override void DoTrigger(Collider other) {
        if (other.transform.gameObject.name.Contains("Wall") || other.transform.gameObject.name.Contains("Door") || other.transform.gameObject.name.Contains("Floor")) {
            Destroy(gameObject);
        }
    }
}
