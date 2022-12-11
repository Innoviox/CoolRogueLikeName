using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Gun : MonoBehaviour
{
    public Transform player;
    public float maxAngle = 80f;
    private Boss1Script boss1;
    private Quaternion rot;
    private LayerMask playerTriggerMelee;
    private void Awake()
    {
        boss1 = transform.parent.transform.GetComponent<Boss1Script>();
        playerTriggerMelee = LayerMask.GetMask("PlayerMelee");
    }

    void Update()
    {
        if (boss1.aimAtPlayer)
        {
            var lookPos = player.position - transform.position;
            var rotation = Quaternion.LookRotation(lookPos);
            rotation *= Quaternion.Euler(new Vector3(90, 0, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
        }
        // Guns aim in the same direction as the bosses body
        else
        {
            rot = transform.parent.rotation;
            rot *= Quaternion.Euler(new Vector3(90, 0, 0));
            transform.rotation = rot;
        }
    }

    // Boss Guns are susceptible to melee damage
    private void OnTriggerEnter(Collider other)
    {
        if (!boss1.immune && InLayer(other, playerTriggerMelee))
        {
            boss1.TakeDamage(other.transform.gameObject.GetComponent<Projectile>().Damage);
        }

    }

    private bool InLayer(Collider other, LayerMask mask)
    {
        return ((1 << other.gameObject.layer) & mask.value) > 0;
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
}
