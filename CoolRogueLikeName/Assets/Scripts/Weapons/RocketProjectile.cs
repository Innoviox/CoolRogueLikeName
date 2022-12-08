using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectile : MonoBehaviour
{
    public int Damage;
    public int Explosion;
    public GameObject explosionRadius;
    public float explosionTime;
    private GameObject explosion;
    // Projectile ends its life when colliding with a wall
    // or an enemy. 
    // Spawns explosion radius and slight VFX to convey an explosion.
    private void OnCollisionEnter(Collision collision)
    {
        // Don't destroy myself if I collide with other bullets
        if (collision.transform.gameObject.name != "RocketProj(Clone)")
        {
            explosion = Instantiate(explosionRadius, collision.contacts[0].point, explosionRadius.transform.rotation);
            // Destroy myself :(
            Destroy(gameObject);

            Destroy(explosion, explosion.GetComponent<AudioSource>().clip.length-0.2F);
        }

    }

    
    private void OnTriggerEnter(Collider other) {
        if (other.transform.gameObject.name.Contains("Wall") || other.transform.gameObject.name.Contains("Door") || other.transform.gameObject.name.Contains("Floor")) {
            Destroy(gameObject);
        }
    }
}
