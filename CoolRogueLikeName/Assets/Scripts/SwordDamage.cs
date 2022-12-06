using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public int Damage;
    

     void Update() 
    {
        float destroyTime = 0.25F;
        
        Destroy(gameObject, destroyTime);
    }

        private void OnTriggerEnter(Collider collision)
    {
        // Don't destroy myself if I collide with other bullets
        if (collision.transform.gameObject.name != "Sword Hitbox(Clone)")
        {
            Debug.Log(Damage);
            //Destroy(gameObject);
        }

    }
    
}
