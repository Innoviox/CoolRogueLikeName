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
        int roomToExpand = expandableRooms[Random.Range(0, expandableRooms.Count)];
        Room room = rooms[roomToExpand];

        if (room == 0)
        {
            // choose every wall in base room
            foreach (int wallToExpand in room.expandableWalls)
            {
                ExpandWall(room, wallToExpand);
            }
        }
        else
        {
            int wallToExpand = room.expandableWalls[Random.Range(0, room.expandableWalls.Count)];
            ExpandWall(room, wallToExpand);
        }

        if (room.expandableWalls.Count == 0)
        {
            expandableRooms.Remove(roomToExpand);
        }
    }

    void ExpandWall(Room room, int wallToExpand)
    {
        int newRoomSize = GenerateRoomSize();
        int newRoomX = 0;
        int newRoomY = 0;

        // todo door offset
        switch (wallToExpand)
        {
            case 0: // north wall
                newRoomX = room.x;
                newRoomY = room.y + room.size + newRoomSize;
                break;
            case 1: // east wall
                newRoomX = room.x + room.size + newRoomSize;
                newRoomY = room.y;
                break;
            case 2: // south wall
                newRoomX = room.x;
                newRoomY = room.y - room.size - newRoomSize;
                break;
            case 3: // west wall
                newRoomX = room.x - room.size - newRoomSize;
                newRoomY = room.y;
                break;
        }

        Room newRoom = Room(newRoomX, newRoomY, newRoomSize);
        rooms.Add(newRoom);
        expandableRooms.Add(rooms.Count - 1);

        // remove wall from expandable walls
        room.expandableWalls.Remove(wallToExpand);
    }

    int GenerateRoomSize()
    {
        return baseRoomSize;
        // return Random.Range(10, 20) / 10.0f * baseRoomSize;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
