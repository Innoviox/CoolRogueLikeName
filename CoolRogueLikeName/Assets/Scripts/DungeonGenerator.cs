using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;


public class DungeonGenerator : MonoBehaviour
{
    public int baseRoomSize = 10;
    public int minRoomSize = 5;
    public int maxRoomSize = 15;
    public int doorSize = 1;
    public int nRooms = 10;
    public List<Transform> blocks;
    public List<Room> rooms;
    public List<int> expandableRooms;
    private Dictionary<string, Transform> blocksDict;
    private int expands;
    private List<Transform> roomBlocks;

    // Start is called before the first frame update
    void Start()
    {
        blocksDict = new Dictionary<string, Transform>();
        foreach (Transform block in blocks)
        {
            blocksDict.Add(block.name, block);
        }

        roomBlocks = new List<Transform>();

        rooms = new List<Room>();
        rooms.Add(new Room(0, 0, baseRoomSize, 0)); // base room

        expandableRooms = new List<int>();
        expandableRooms.Add(0); // base room is expandable

        expands = nRooms - 1;
    }

    void ExpandN(int n)
    { // expand N times
        for (int i = 0; i < n; i++)
        {
            Expand();
        }
    }

    void Expand()
    {
        int roomToExpand = expandableRooms[Random.Range(0, expandableRooms.Count)];
        Room room = rooms[roomToExpand];

        if (roomToExpand == 0) // choose every wall in base room
        {
            foreach (int wallToExpand in new List<int>(room.expandableWalls))
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
        Debug.Log($"Expanding room {room.id} wall {wallToExpand}");
        int maxSize = GetMaxSize(room, wallToExpand);
        if (maxSize == 0)
        {
            return; // not enough room here, skip expand
        }

        int newRoomSize = GenerateRoomSize(maxSize);
        int roomOffset = GenerateRoomOffset(room.size, newRoomSize, maxSize);

        int newRoomX = 0;
        int newRoomY = 0;

        int oppositeWall = (wallToExpand + 2) % 4;
        int unGuaranteeableWall = (wallToExpand + 1) % 4;

        // todo door offset
        switch (wallToExpand)
        {
            case 0: // north wall
                newRoomX = room.x - roomOffset;
                newRoomY = room.y + room.size + newRoomSize;
                break;
            case 1: // east wall
                newRoomX = room.x + room.size + newRoomSize;
                newRoomY = room.y + roomOffset;
                break;
            case 2: // south wall
                newRoomX = room.x + roomOffset;
                newRoomY = room.y - room.size - newRoomSize;
                break;
            case 3: // west wall
                newRoomX = room.x - room.size - newRoomSize;
                newRoomY = room.y - roomOffset;
                break;
        }

        Room newRoom = new Room(newRoomX, newRoomY, newRoomSize, rooms.Count);
        newRoom.expandableWalls.Remove(oppositeWall); // can't expand into the room we just came from
        newRoom.expandableWalls.Remove(unGuaranteeableWall); // a room could generate at this wall, don't expand into it

        rooms.Add(newRoom);
        expandableRooms.Add(rooms.Count - 1);

        // remove wall from expandable walls
        room.expandableWalls.Remove(wallToExpand);

        Debug.Log($"Made room {newRoom.id} size {room.size} offset {roomOffset}");
    }

    int GetMaxSize(Room room, int wallToExpand)
    {
        int maxSize = (int)minRoomSize;

        while (maxSize < maxRoomSize * 2)
        {
            maxSize++;

            if (InOtherRoom(room, wallToExpand, maxSize))
            {
                break;
            }
        }
        maxSize /= 2;

        Debug.Log($"Found Max size: {maxSize}");

        if (maxSize < minRoomSize)
        {
            return 0;
        }

        return maxSize;
    }

    int GenerateRoomSize(int maxSize)
    {
        return (int)Random.Range(minRoomSize, maxSize);
    }

    bool InOtherRoom(Room room, int wallToExpand, int size)
    {
        // note that here size is double what it normally is (eg diameter not radius)
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
                    y1 = room.y + room.size + size;
                    break;
                case 1: // east wall
                    x1 = room.x + room.size;
                    y1 = room.y - room.size + size;
                    break;
                case 2: // south wall
                    x1 = room.x - room.size;
                    y1 = room.y - room.size;
                    break;
                case 3: // west wall
                    x1 = room.x - room.size - size;
                    y1 = room.y + room.size;
                    break;
            }

            int x2 = x1 + size;
            int y2 = y1 - size;

            if (r.Overlaps(x1, y1, x2, y2))
            {
                return true;
            }
        }

        return false;
    }

    int GenerateRoomOffset(int oldRoomSize, int newRoomSize, int maxSize)
    {
        Debug.Log($"Min Offset {newRoomSize - oldRoomSize} Max Offset {newRoomSize + oldRoomSize - doorSize * 2}");
        return (int)Random.Range(newRoomSize - oldRoomSize, 2 * maxSize - newRoomSize - oldRoomSize);
    }

    void MakeDungeon()
    {
        foreach (Room room in rooms)
        {
            roomBlocks.AddRange(room.MakeRoom(blocksDict));
        }
    }

    void ClearDungeon()
    {
        foreach (Transform block in roomBlocks)
        {
            Destroy(block.gameObject);
        }
        roomBlocks.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearLog();
            ClearDungeon();
            Start();
            ExpandN(expands);
            MakeDungeon();
        }
    }

    // https://stackoverflow.com/questions/40577412/clear-editor-console-logs-from-script
    public void ClearLog()
    {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }


}
