using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : MonoBehaviour
{
    public List<GameObject> WeaponList;
    public Transform hinge;
    public string openKey;
    public float openDuration;

    public GameObject availableWeapon;

    bool opened = false;
    bool weaponReady = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerStay(Collider other)
    {
        if (other.name == "Player(Clone)")
        {
            if (opened)
            {
                if (weaponReady)
                {
                    if (Input.GetKeyDown(openKey))
                    {
                        // TODO: swap out weapons on player
                        // be sure to enable/disable the weapon scripts
                        Debug.Log("swapping weapons (unimplemented)");
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(openKey))
                {
                    StartCoroutine(OpenChest());
                    opened = true;
                }
            }
        }
    }

    IEnumerator OpenChest()
    {
        Quaternion openQ = Quaternion.Euler(-30, 90, 90);
        Quaternion startQ = hinge.rotation;
        float timeTaken = 0;
        while (hinge.rotation != openQ)
        {
            hinge.rotation = Quaternion.Lerp(startQ, openQ, timeTaken / openDuration);
            timeTaken += Time.deltaTime;
            yield return null;
        }

        //availableWeapon = Instantiate(WeaponList[Random.Range(0, WeaponList.Count)]);
        availableWeapon = Instantiate(WeaponList[0]);
        Transform weaponTransform = availableWeapon.GetComponent<Transform>();
        weaponTransform.parent = transform;
        weaponTransform.position = new Vector3(0, 0.5f, 0);
        weaponReady = true;
    }
}
