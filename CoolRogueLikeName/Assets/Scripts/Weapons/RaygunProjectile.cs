using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaygunProjectile : Projectile
{
    // Start is called before the first frame update
    void Awake()
    {
        mask = LayerMask.GetMask("Wall", "Door", "Floor");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
