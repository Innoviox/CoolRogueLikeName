using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseWeapon : MonoBehaviour
{
    public Transform spawnPoint;   // Select this through the inspector
    public GameObject projectile;  // Selected Bullet prefab through inspector
    public float baseProjectileSpeed = 5;
    public float baseCooldown = 0.25f;
    public float baseDamage = 1;
    public PowerupManager stats;
    public string firebutton;
    private IEnumerator weaponCoroutineObject;
    public Slider weaponSlider;

    public void EnableWeapon()
    {
        weaponCoroutineObject = WeaponCoroutine();
        StartCoroutine(weaponCoroutineObject);
    }
    public void DisableWeapon()
    {
        StopCoroutine(weaponCoroutineObject);
    }

    IEnumerator WeaponCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => Input.GetButtonDown(firebutton));
            DoWeaponAction(baseDamage * stats.playerDamageFactor, baseProjectileSpeed * stats.bulletSpeedFactor);

            weaponSlider.value = 0;
            weaponSlider.maxValue = baseCooldown * stats.playerReloadSpeedFactor;
            for (float i = 0; i < weaponSlider.maxValue; i += Time.deltaTime)
            {
                weaponSlider.value = i;
                yield return null;
            }
            weaponSlider.value = weaponSlider.maxValue;
        }
    }

    // Don't do any powerup scaling, it has already been done
    public virtual void DoWeaponAction(float damage, float projectileSpeed)
    {
        // Create a bullet from the prefab 
        GameObject bullet = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        GetComponent<AudioSource>().Play();
        // Set projectiles damage
        bullet.GetComponent<Projectile>().Damage = baseDamage;

        // Set its velocity to go forward by projectileSpeed
        bullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * projectileSpeed;
    }

    public void SetHud(Slider slider)
    {
        weaponSlider = slider;
        weaponSlider.maxValue = baseCooldown * stats.playerReloadSpeedFactor;
        weaponSlider.value = weaponSlider.maxValue;
    }
}
