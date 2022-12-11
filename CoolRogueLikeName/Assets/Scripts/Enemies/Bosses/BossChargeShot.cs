using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeShot : EnemyProjectile
{
    public GameObject enemyProj;
    public int numBulletsPerRow;  // Number of bullets along one the arc
    public int numBulletsPerCol;
    private float degreeToRotate; // Evenly distributes bullets along 180 degrees
    private LayerMask maskScatter;

    // Hides parents Awake
    protected override void Awake()
    {
        numBulletsPerRow = 20;
        numBulletsPerCol = 3;
        maskPlayer = LayerMask.GetMask("PlayerBody");
        maskScatter = LayerMask.GetMask("Wall", "Door");
        startTime = Time.time;
        degreeToRotate = 180 / (numBulletsPerRow + 1);
    }
  
    protected override void OnTriggerEnter(Collider other)
    {
        if (InLayer(other, maskPlayer))
        {
            Destroy(gameObject);
        }

        // When colliding with a wall, spawn an arc of bullets going in the direction opposite of the wall.
        if (InLayer(other, maskScatter))
        {
            // Get bullets direction
            Vector3 currDir;

            switch (other.transform.rotation.eulerAngles.y)
            {
                case 0:
                    currDir = Vector3.back;
                    break;
                case 90:
                    currDir = Vector3.left;
                    break;
                case 180:
                    currDir = Vector3.forward;
                    break;
                case 270:
                    currDir = Vector3.right;
                    break;
                default:
                    currDir = Vector3.forward;
                    break;
            }

            // bullet position
            Vector3 pos = transform.localPosition;
            GameObject bullet;

            for (int i = 0; i < numBulletsPerRow; i++)
            {
                // Number of bullets per column on explosion
                for (int j = 1; j <= numBulletsPerCol; j++)
                {
                    pos += j * currDir; // spawn location of bullet on rows 1 through numBulletsPerCol 

                    bullet = Instantiate(enemyProj, pos, transform.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = currDir * 3;
                    pos = transform.localPosition; // Reset position
                }

                currDir = Quaternion.Euler(0, degreeToRotate, 0) * currDir;
            }

            Destroy(gameObject);
        }
    }

    private bool InLayer(Collider other, LayerMask mask)
    {
        return ((1 << other.gameObject.layer) & mask.value) > 0;
    }
}
