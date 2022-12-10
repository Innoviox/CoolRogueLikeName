using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponHolder : MonoBehaviour
{

    public Vector3 weaponPosition;
    public Transform weapon;
    public Slider weaponSlider;

    void Start()
    {
        //weapon.position = weaponPosition;
        weapon.rotation = Quaternion.identity;
        weapon.gameObject.GetComponent<BaseWeapon>().EnableWeapon();
    }

    // swap all details of the transforms
    public Transform SwapWeapons(Transform newWeapon)
    {
        Vector3 tempPos = newWeapon.position;
        newWeapon.position = weapon.position;
        weapon.position = tempPos;

        Quaternion tempRot = newWeapon.rotation;
        newWeapon.rotation = weapon.rotation;
        weapon.rotation = tempRot;

        Transform tempParent = newWeapon.parent;
        newWeapon.parent = weapon.parent;
        weapon.parent = tempParent;

        // todo: enable & disable weapon shooting
        newWeapon.gameObject.GetComponent<BaseWeapon>().EnableWeapon();
        weapon.gameObject.GetComponent<BaseWeapon>().DisableWeapon();

        Transform tempWeapon = weapon;
        weapon = newWeapon;
        weapon.gameObject.GetComponent<BaseWeapon>().SetHud(weaponSlider);
        return tempWeapon;
    }

    public void SetWeapon(Transform newWeapon)
    {
        newWeapon.parent = transform;
        newWeapon.position = weaponPosition;
        newWeapon.rotation = Quaternion.identity;
        weapon = newWeapon;
        weapon.gameObject.GetComponent<BaseWeapon>().SetHud(weaponSlider);
    }

    public Transform GetWeapon()
    {
        return weapon;
    }

    public void SetHud(Slider slider)
    {
        weaponSlider = slider;
        weapon.gameObject.GetComponent<BaseWeapon>().SetHud(weaponSlider);
    }
}
