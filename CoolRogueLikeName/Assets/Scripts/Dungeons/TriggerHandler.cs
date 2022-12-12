using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // allow room to have both a trigger collider and a regular collider
        transform.parent.GetComponent<DungeonRoomScript>().OnTriggerEnter(other);
    }
}
