using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : Projectile
{
    public GameObject explosionRad;
   
    public float expTime;
    // Start is called before the first frame update
    void Start()
    {
        Light explosionLight = explosionRad.AddComponent<Light>();
        explosionLight.type = LightType.Point;
        explosionLight.color = new Color(0.7f, 0.5f, 0.0f, 0.7f);
        GetComponent<AudioSource>().Play();
        expTime = explosionRad.GetComponent<AudioSource>().clip.length;
        Destroy(gameObject, GetComponent<AudioSource>().clip.length - 0.2F);
    }

    // Update is called once per frame
    void Update()
    {
        
    
    }

    protected override void DoTrigger(Collider other)
    {
        // do nothing
        Debug.Log("Called explosions trigger, other =" + other.name);
    }

    protected override void DoCollision(Collision collision)
    {
        // do nothing
        Debug.Log("Called explosions trigger, other =" + collision.gameObject.name);

    }

}
