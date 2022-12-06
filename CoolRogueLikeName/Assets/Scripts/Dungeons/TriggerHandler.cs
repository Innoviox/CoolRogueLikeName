using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        transform.parent.GetComponent<DungeonRoomScript>().OnTriggerEnter(other);
    }
}
