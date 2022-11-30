using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public int baseRoomSize = 10;
    public float minRoomSize = 5.0f;
    public float maxRoomSize = 15.0f;
    public int doorSize = 1;
    public List<Room> rooms;
    public List<int> expandableRooms;

    // Start is called before the first frame update
    void Start()
    {
        rooms = new List<Room>();
        rooms.Add(new Room(0, 0, baseRoomSize, 0)); // base room

        expandableRooms = new List<int>();
        expandableRooms.Add(0); // base room is expandable

        Expand(); // make 4 starting rooms

    }

    void Expand()
    {
        int roomToExpand = expandableRooms[Random.Range(0, expandableRooms.Count)];
        Room room = rooms[roomToExpand];

        if (roomToExpand == 0) // choose every wall in base room
        {
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
        int newRoomSize = GenerateRoomSize(room, wallToExpand);
        int roomOffset = GenerateRoomOffset();

        int newRoomX = 0;
        int newRoomY = 0;

        int oppositeWall = (wallToExpand + 2) % 4;
        int unGuaranteeableWall = (wallToExpand + 1) % 4;

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

        Room newRoom = new Room(newRoomX, newRoomY, newRoomSize, rooms.Count);
        newRoom.expandableWalls.Remove(oppositeWall); // can't expand into the room we just came from
        newRoom.expandableWalls.Remove(unGuaranteeableWall); // a room could generate at this wall, don't expand into it

        rooms.Add(newRoom);
        expandableRooms.Add(rooms.Count - 1);

        // remove wall from expandable walls
        room.expandableWalls.Remove(wallToExpand);
    }

    int GenerateRoomSize(Room room, int wallToExpand)
    {
        // todo do something cool here
        // return (int)(Random.Range(minRoomSize, maxRoomSize)); // room sizes are ints so the block system works
        int maxSize = (int)minRoomSize;

        while (maxSize < maxRoomSize)
        {
            maxSize++;

            if (InOtherRoom(room, wallToExpand, maxSize))
            {
                break;
            }
        }

        return (int)Random.Range(minRoomSize, maxSize);
    }

    bool InOtherRoom(Room room, int wallToExpand, int size)
    {
        foreach (Room r in rooms)
        {
            if (r.id == room.id)
            {
                continue;
            }

            int x1 = 0;
            int y1 = 0;

            switch (wallToExpand)
            {
                case 0: // north wall
                    x1 = room.x + room.size - size;
                    y1 = room.y + room.size;
                    break;
                case 1: // east wall
                    x1 = room.x + room.size + size;
                    y1 = room.y - room.size;
                    break;
                case 2: // south wall
                    x1 = room.x - room.size + size;
                    y1 = room.y - room.size;
                    break;
                case 3: // west wall
                    x1 = room.x - room.size - size;
                    y1 = room.y + room.size;
                    break;
            }

            int x2 = 0;
            int y2 = 0;

            switch (wallToExpand)
            {
                case 0: // north wall
                    x2 = room.x + room.size;
                    y2 = room.y + room.size + size;
                    break;
                case 1: // east wall
                    x2 = room.x + room.size;
                    y2 = room.y - room.size + size;
                    break;
                case 2: // south wall
                    x2 = room.x - room.size;
                    y2 = room.y - room.size - size;
                    break;
                case 3: // west wall
                    x2 = room.x - room.size;
                    y2 = room.y + room.size - size;
                    break;
            }

            if (r.InRoom(x1, y1) || r.InRoom(x2, y2))
            {
                return true;
            }
        }

        return false;
    }

    int GenerateRoomOffset(Room oldRoom, Room newRoom)
    {
        return (int)Random.Range(newRoom.size - oldRoom.size, newRoom.size + oldRoom.size - doorSize * 2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
