using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : Projectile
{
    public float lifetime = 0.25f;

    void Start() 
    {
        Destroy(gameObject, lifetime);
    }

    protected override void DoTrigger(Collider collision)
    {
        // Don't destroy myself if I collide with other things
    }
    protected override void DoCollision(Collision collision)
    {
        // Don't destroy myself if I collide with other things
    }
}
