using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChargeShot : MonoBehaviour
{
    public GameObject enemyProj;
    public Vector3 scaleChange, positionChange;
    Rigidbody body;
    public float Damage;
    public int numBulletsPerRow;  // Number of bullets along one the arc
    public int numBulletsPerCol;
    private float degreeToRotate; // Evenly distributes bullets along 180 degrees

    void Start()
    {
        body = GetComponent<Rigidbody>();
        numBulletsPerRow = 20;
        numBulletsPerCol = 3;

        degreeToRotate = 180 / (numBulletsPerRow + 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "Player")
        {
            // damage player

            // Destroy self
            Destroy(gameObject);
        }

        // When colliding with a wall, spawn an arc of bullets going in the direction opposite of the wall.
        if (other.transform.parent != null && other.transform.parent.transform.name == "Walls")
        {
            // Get bullets direction
            Vector3 currDir; 
 
            // We want the vector normal to the surface of the wall pointing towards the inside of the room
            if (Vector3.Dot(body.velocity, other.transform.forward) < 0)
                currDir = other.transform.forward;
            else
                currDir = other.transform.forward * -1;

            // Initial spawn direction for smaller bullets
            currDir = Quaternion.Euler(0, -90 + degreeToRotate, 0) * currDir;

            // bullet position
            Vector3 pos = transform.localPosition;
            GameObject bullet;

            // Spawn 3 bullets 45 degrees from each other. 
            for (int i = 0; i < numBulletsPerRow; i ++)
            {
                // Number of bullets per column on explosion
                for (int j = 1; j <= numBulletsPerCol; j++)
                {
                    pos += j * currDir; // spawn location of bullet on rows 1 through numBulletsPerCol 

                    bullet = Instantiate(enemyProj, pos, transform.rotation);
                    bullet.GetComponent<EnemyProjectile>().Damage = 5;
                    bullet.GetComponent<Rigidbody>().velocity = currDir * 3;
                    pos = transform.localPosition; // Reset position
                }

                currDir = Quaternion.Euler(0, degreeToRotate, 0) * currDir;
            }

            Destroy(gameObject);
        }
    }
}
