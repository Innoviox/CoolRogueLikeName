using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
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

    }

    // Update is called once per frame
    void Update()
    {
        
    
    }
}
