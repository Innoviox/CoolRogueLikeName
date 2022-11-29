using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int baseRoomSize = 10;
    public List<Room> rooms;
    public List<int> expandableRooms;

    // Start is called before the first frame update
    void Start()
    {
        rooms = new List<Room>();
        rooms.Add(Room(0, 0, baseRoomSize)); // base room

        expandableRooms = new List<int>();
        expandableRooms.Add(0); // base room is expandable
    }

    void Expand()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
