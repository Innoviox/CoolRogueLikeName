using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossWave : MonoBehaviour
{
    // With Update() growth depends on lag
    // Scale change: 0.02
    // Position Change: 0.01

    // Changed to FixedUpdate so it has less variance on lag
    // Scale change: 0.08     0.16
    // Position Change: 0.04  0.08 also feels good

    public int WaveSpeed;
    private float scale, pos;
    private Vector3 scaleC, posC;
    public float timeTolive;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        SetScalePos();
    }

    private void FixedUpdate()
    {
        transform.localScale += scaleC;
        transform.localPosition += posC;

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

    /// <summary>
    /// Translate Wave Speed into scale change and position change
    /// </summary>
    private void TranslateSpeed()
    {
        scale = (float)WaveSpeed;
        scale /= 100;
        pos = scale / 2;
    }

    private void SetScalePos()
    {
        TranslateSpeed();

        switch (gameObject.name)
        {
            case "WaveLeft":
                scaleC = new Vector3(scale, 0, 0);
                posC = new Vector3(-pos, 0, 0);
                break;
            case "WaveRight":
                scaleC = new Vector3(scale, 0, 0);
                posC = new Vector3(pos, 0, 0);
                break;
            case "WaveFront":
                scaleC = new Vector3(scale, 0, 0);
                posC = new Vector3(0, 0, pos);
                break;
            default:
                scaleC = new Vector3(scale, 0, 0);
                posC = new Vector3(0, 0, -pos);
                break;
        }
    }
}
