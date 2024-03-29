using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : MonoBehaviour
{
    public List<GameObject> WeaponList;
    public Vector3 weaponFloatPosition;
    public Transform hinge;
    public string openKey;
    public float openDuration;

    public Transform availableWeaponTransform;
    public float rotateSpeed;

    bool opened = false;
    bool weaponReady = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (weaponReady)
        {
            availableWeaponTransform.RotateAround(transform.position, Vector3.up, rotateSpeed);
        }
    }

    // Update is called once per frame
    void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            if (opened)
            {
                if (weaponReady)
                {
                    if (Input.GetKeyDown(openKey))
                    {
                        PlayerWeaponHolder holder = other.gameObject.GetComponent<PlayerWeaponHolder>();
                        availableWeaponTransform = holder.SwapWeapons(availableWeaponTransform);
                    }
                }
            }
            else
            {
                if (Input.GetKey(openKey))
                {
                    StartCoroutine(OpenChest());
                    opened = true;
                }
            }
        }
    }

    IEnumerator OpenChest()
    {
        int weaponIndex = Random.Range(0, WeaponList.Count);

        Quaternion openQ = hinge.rotation * Quaternion.Euler(0, 120, 0);
        Quaternion startQ = hinge.rotation;
        float timeTaken = 0;
        while (timeTaken < openDuration)
        {
            hinge.rotation = Quaternion.Lerp(startQ, openQ, timeTaken / openDuration);
            timeTaken += Time.deltaTime;
            yield return null;
        }
        hinge.rotation = openQ;
        availableWeaponTransform = GameObject.Instantiate(WeaponList[weaponIndex]).GetComponent<Transform>();
        availableWeaponTransform.parent = transform;
        availableWeaponTransform.SetLocalPositionAndRotation(weaponFloatPosition, Quaternion.identity);
        weaponReady = true;
    }
}
