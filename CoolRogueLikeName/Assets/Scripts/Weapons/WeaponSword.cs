using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSword : BaseWeapon
{
    public override void DoWeaponAction(float damage, float projectileSpeed)
    {
        // This section is the same as the base, but without needing to access the rigidbody of the projectile
        // Create a bullet from the prefab 
        GameObject bullet = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        GetComponent<AudioSource>().Play();
        // Set projectiles damage
        bullet.GetComponent<Projectile>().Damage = baseDamage;
        StartCoroutine(Rotate(Vector3.up, 0.25f));
    }

    IEnumerator Rotate(Vector3 axis, float duration)
    {
        Quaternion startRotation = spawnPoint.localRotation;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            spawnPoint.Rotate(axis * -360/duration * Time.deltaTime);
            yield return null;
        }
        spawnPoint.localRotation = startRotation;
    }
}
