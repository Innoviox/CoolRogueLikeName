using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSword : MonoBehaviour
{
    public Transform spawnPoint;   // Select this through the inspector
    public GameObject projectile;  // Selected Invisible hitbox prefab through inspector
    public Transform rotateAbout;
    public int baseDamage; // set in inspector for easy testing
    //float timer = 30.0f;
    public float attackRate;
    public float cooldown;
    private float nextAttack;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > nextAttack)
        {
            
            nextAttack = Time.time + cooldown;

            // Create a bullet from the prefab 

            GameObject hitbox = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
            StartCoroutine( Rotate(Vector3.up, 40, attackRate) );
            GetComponent<AudioSource>().Play();
            //hitbox.transform.SetParent(spawnPoint);
            // Set projectiles damage
            hitbox.GetComponent<SwordDamage>().Damage = baseDamage;

        
        }
    }

    IEnumerator Rotate( Vector3 axis, float angle, float duration)
   {
      float startRotation = rotateAbout.eulerAngles.y;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while ( t  < duration )
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            rotateAbout.eulerAngles = new Vector3(rotateAbout.eulerAngles.x, yRotation, rotateAbout.eulerAngles.z);
            yield return null;
        }
   }
 }
