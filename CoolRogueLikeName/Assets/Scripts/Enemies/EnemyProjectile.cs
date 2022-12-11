using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float Damage;
    public bool breakable; // If this projectile can be destroyed by other projectiles.
    public LayerMask maskPlayer;
    private LayerMask environment;
    private LayerMask maskBreakable;

    private void Awake()
    {
        maskPlayer = LayerMask.GetMask("PlayerBody");
        environment = LayerMask.GetMask("Wall", "Door", "Floor");
        maskBreakable = LayerMask.GetMask("PlayerBody", "Wall", "Door", "PlayerProjectile", "PlayerMelee");
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (breakable && InLayer(other, maskBreakable))
        {
            Destroy(gameObject);
        }
        else if (InLayer(other, maskPlayer) || InLayer(other, environment))
        {
            Destroy(gameObject);
        }
    }
    
    private bool InLayer(Collider other, LayerMask mask)
    {
        return ((1 << other.gameObject.layer) & mask.value) > 0;
    }

}
