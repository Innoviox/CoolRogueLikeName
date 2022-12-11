using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossWave : EnemyProjectile
{
    public int WaveSpeed;
    private float scale, pos;
    private Vector3 scaleC, posC;

    protected override void Awake()
    {
        startTime = Time.time;
        SetScalePos();
    }

    protected override void FixedUpdate()
    {
        transform.localScale += scaleC;
        transform.localPosition += posC;

        // Destroy wave if enough time has passed
        if (startTime + timeTolive < Time.time)
            Destroy(gameObject.transform.parent.gameObject);
    }

    // Wave is only destroyed after a set amount of time
    protected override void OnTriggerEnter(Collider other) { }

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
