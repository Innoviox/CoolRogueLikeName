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

        StartCoroutine(Rotate(Vector3.up, 40, baseCooldown * stats.playerReloadSpeedFactor));
    }

    IEnumerator Rotate(Vector3 axis, float angle, float duration)
    {
        float startRotation = spawnPoint.eulerAngles.y;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            spawnPoint.eulerAngles = new Vector3(spawnPoint.eulerAngles.x, yRotation, spawnPoint.eulerAngles.z);
            yield return null;
        }
    }
}
