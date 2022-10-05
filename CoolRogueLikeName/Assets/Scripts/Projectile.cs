using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage;

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
}
