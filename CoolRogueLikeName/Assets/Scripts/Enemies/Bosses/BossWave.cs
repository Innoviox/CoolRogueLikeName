using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossWave : MonoBehaviour
{
  
    public Vector3 scaleChange, positionChange; // Scale should be twice as much as the absolute value of position
    private float timeTolive;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        timeTolive = 9;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += scaleChange;
        transform.localPosition += positionChange;

        // Destroy wave if enough time has passed
        if (startTime + timeTolive < Time.time)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player")
        {
            // Damage player 
            // Call players method to take damage
        }
    }
}
