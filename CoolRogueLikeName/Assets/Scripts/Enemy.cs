using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    int health = 5;

    // Take damage on colliding with projectile
    private void OnCollisionEnter(Collision collision)
    {
        // Check collision occured with a bullet
        // Since bullet is a prefab its gameobject name gets set to Bullet(Clone)
        if (collision.transform.gameObject.name == "Bullet(Clone)")
        {
            // Get the projectiles damage
            int damageTaken = collision.transform.gameObject.GetComponent<Projectile>().Damage;

            health -= damageTaken;
        }

        if (health <= 0)
            Destroy(gameObject);
    }
}
