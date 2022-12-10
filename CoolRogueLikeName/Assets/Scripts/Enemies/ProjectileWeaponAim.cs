using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponAim : MonoBehaviour
{
    public Transform player; 
 
    // Update is called once per frame
    void Update()
    {
        var lookPos = player.position - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);

        // Since current projectile weapons are rotated 90 degrees we need to add this
        rotation *= Quaternion.Euler(new Vector3(90, 0, 0)); 
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
}
