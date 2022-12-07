using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHolder : MonoBehaviour
{

    public Vector3 weaponPosition;
    public Transform weapon;

    void Start()
    {
        //weapon.position = weaponPosition;
        weapon.rotation = Quaternion.identity;
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

        Transform tempWeapon = weapon;
        weapon = newWeapon;
        return tempWeapon;
    }

    public void SetWeapon(Transform newWeapon)
    {
        newWeapon.parent = transform;
        newWeapon.position = weaponPosition;
        newWeapon.rotation = Quaternion.identity;
        weapon = newWeapon;
    }

    public Transform GetWeapon()
    {
        return weapon;
    }
}
