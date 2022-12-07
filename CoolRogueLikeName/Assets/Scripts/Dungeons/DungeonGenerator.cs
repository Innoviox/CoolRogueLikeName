using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using WallMethods;


public class DungeonGenerator : MonoBehaviour
{
    public int baseRoomSize = 10;
    public int minRoomSize = 5;
    public int maxRoomSize = 12;
    public int doorSize = 1;
    public int nRooms = 10;
    public int bossSize = 15;
    public List<Transform> blocks; // id prefer this to be a dict but unity doesnt do dicts in the inspector
    private Dictionary<string, Transform> blocksDict;
    public List<Room> rooms;
    public List<int> expandableRooms;
    public new Camera camera;
    public Transform playerPrefab;
    private Transform player;
    private int expands;
    private List<Transform> roomBlocks;
    private List<Transform> dungeonRooms;
    private List<Door> globalDoorLocations;
    private bool started = false;
    private RoomGenerator roomGenerator;
    private Transform teleporter = null;

    // Start is called before the first frame update
    void Start()
    {
        blocksDict = new Dictionary<string, Transform>();
        foreach (Transform block in blocks)
        {
            blocksDict.Add(block.name, block);
        }
        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        roomBlocks = new List<Transform>();
        rooms = new List<Room>();
        globalDoorLocations = new List<Door>();
        dungeonRooms = new List<Transform>();

        roomGenerator = GetComponent<RoomGenerator>();
        roomGenerator.Setup();

        Teleport();
    }

    Room MakeRoom(int x, int y, int size, int depth)
    {
        var room = new Room(x, y, size, depth);
        rooms.Add(room);
        expandableRooms.Add(rooms.Count - 1);

        return room;
    }

    void ExpandN(int n)
    { // expand N times
        for (int i = 0; i < n; i++)
        {
            Expand(false);
        }
    }

    void Expand(bool bossRoom)
    {
        int roomToExpand = expandableRooms[UnityEngine.Random.Range(0, expandableRooms.Count)];
        Room room = rooms[roomToExpand];
        Wall wallToExpand = room.expandableWalls[UnityEngine.Random.Range(0, room.expandableWalls.Count)];

        int tries = 0, maxTries = bossRoom ? 100 : 10; // we really wanna generate the boss room
        while (!ExpandWall(room, wallToExpand, bossRoom) && tries < maxTries)
        {
            roomToExpand = expandableRooms[UnityEngine.Random.Range(0, expandableRooms.Count)];
            room = rooms[roomToExpand];
            wallToExpand = room.expandableWalls[UnityEngine.Random.Range(0, room.expandableWalls.Count)];
            tries++;
        }

        if (room.expandableWalls.Count == 0)
        {
            expandableRooms.Remove(roomToExpand);
        }
    }

    bool ExpandWall(Room room, Wall wallToExpand, bool bossRoom)
    {
        // Debug.Log($"Expanding room {room.id} wall {wallToExpand}");

        int maxSize = GetMaxSize(room, wallToExpand, bossRoom);
        if (maxSize == 0 || (bossRoom && maxSize < bossSize))
        {
            return false; // not enough room here, skip expand
        }

        int newRoomSize = bossRoom ? bossSize : GenerateRoomSize(maxSize);
        int roomOffset = GenerateRoomOffset(room.size, newRoomSize, maxSize);

        int newRoomX = 0;
        int newRoomY = 0;

        Wall oppositeWall = wallToExpand.Opposite();
        Wall unGuaranteeableWall = wallToExpand.UnGuaranteeable();

        // todo door offset
        switch (wallToExpand)
        {
            case Wall.North: // north wall
                newRoomX = room.x - roomOffset;
                newRoomY = room.y + room.size + newRoomSize;
                break;
            case Wall.East: // east wall
                newRoomX = room.x + room.size + newRoomSize;
                newRoomY = room.y + roomOffset;
                break;
            case Wall.South: // south wall
                newRoomX = room.x + roomOffset;
                newRoomY = room.y - room.size - newRoomSize;
                break;
            case Wall.West: // west wall
                newRoomX = room.x - room.size - newRoomSize;
                newRoomY = room.y - roomOffset;
                break;
        }

        Room newRoom = MakeRoom(newRoomX, newRoomY, newRoomSize, rooms.Count);
        newRoom.expandableWalls.Remove(oppositeWall); // can't expand into the room we just came from
        newRoom.expandableWalls.Remove(unGuaranteeableWall); // a room could generate at this wall, don't expand into it

        // remove wall from expandable walls
        room.expandableWalls.Remove(wallToExpand);

        // Debug.Log($"Made room {newRoom.id} size {newRoom.size} offset {roomOffset}");

        return true;
    }

    int GetMaxSize(Room room, Wall wallToExpand, bool bossRoom)
    {
        int maxSize = (int)minRoomSize;
        int maximum = bossRoom ? bossSize : maxRoomSize;

        while (maxSize < maximum * 2)
        {
            maxSize++;

            if (InOtherRoom(room, wallToExpand, maxSize))
            {
                break;
            }
        }
        maxSize /= 2;

        // Debug.Log($"Found Max size: {maxSize}");

        if (maxSize < minRoomSize)
        {
            return 0;
        }

        return maxSize;
    }

    int GenerateRoomSize(int maxSize)
    {
        return (int)UnityEngine.Random.Range(minRoomSize, maxSize);
    }

    bool InOtherRoom(Room room, Wall wallToExpand, int size)
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
                case Wall.North: // north wall
                    x1 = room.x + room.size - size;
                    y1 = room.y + room.size + size;
                    break;
                case Wall.East: // east wall
                    x1 = room.x + room.size;
                    y1 = room.y - room.size + size;
                    break;
                case Wall.South: // south wall
                    x1 = room.x - room.size;
                    y1 = room.y - room.size;
                    break;
                case Wall.West: // west wall
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
        int max1 = newRoomSize + oldRoomSize - doorSize * 2;
        int max2 = 2 * maxSize - newRoomSize - oldRoomSize;
        return (int)UnityEngine.Random.Range(newRoomSize - oldRoomSize, Mathf.Min(max1, max2));
    }

    void GenerateDoors()
    {
        var doors = new Dictionary<int, List<Wall>>(); // dict from room id to list of walls with doors
        foreach (Room room1 in rooms)
        {
            foreach (Room room2 in rooms)
            {
                if (room1.id == room2.id) continue;

                Wall wall = room1.SharedWall(room2);
                if (wall == Wall.None) continue;
                if (doors.ContainsKey(room2.id) && doors[room2.id].Contains(wall.Opposite())) continue;

                // room1.AddDoor(room1.GenerateDoorLocation(wall, room2));
                var location = room1.GenerateDoorLocation(wall, room2);
                globalDoorLocations.Add(new Door((int)location.x, (int)location.y, room1.id, room2.id, wall));

                if (doors.ContainsKey(room1.id))
                {
                    doors[room1.id].Add(wall);
                }
                else
                {
                    doors[room1.id] = new List<Wall> { wall };
                }

                // Debug.Log($"Door on wall {wall} of room {room1.id} going to {room2.id}");
            }
        }

        foreach (Room room in rooms)
        {
            room.doorLocations = globalDoorLocations;
        }
    }

    void GenerateDungeon()
    {
        try
        {
            ExpandN(expands);
            Expand(true); // expand the boss room
            GenerateDoors();
        }
        catch (Exception e)
        {
            Debug.Log($"dungeon failed, retrying {e}");
            Reset();
            GenerateDungeon();
        }
    }

    void MakeDungeon()
    {
        foreach (Room room in rooms)
        {
            Transform drt = room.MakeRoom(blocksDict, player, camera);
            roomGenerator.GenerateRoom(drt, new Vector3(room.x, 0, room.y), room.size, room.size, globalDoorLocations);

            drt.parent = GetComponent<Transform>();

            foreach (Door door in globalDoorLocations)
            {
                drt.GetComponent<DungeonRoomScript>().AddDelegates(door);
            }

            dungeonRooms.Add(drt);
        }

        MakeDoors();
        MakeTeleporter();
    }

    void MakeDoors()
    {
        foreach (Door door in globalDoorLocations)
        {
            Transform prefab = blocksDict["Door"];
            Transform doorTransform = GameObject.Instantiate(prefab, new Vector3(door.x, 0, door.y), Quaternion.identity);
            doorTransform.name = $"Door ({door.x}, {door.y})";

            var drs = doorTransform.GetComponent<DungeonDoorScript>();
            drs.player = player;
            drs.roomThisDoorLeadsFrom = dungeonRooms[door.room1].GetComponent<DungeonRoomScript>();
            drs.roomThisDoorLeadsTo = dungeonRooms[door.room2].GetComponent<DungeonRoomScript>();

            door.doorTransform = doorTransform;
        }
    }

    void MakeTeleporter()
    {
        Vector3 loc = rooms[rooms.Count - 1].RandomLocation(2.0f);
        Transform prefab = blocksDict["Teleporter"];
        teleporter = GameObject.Instantiate(prefab, loc, Quaternion.identity);
        teleporter.name = "Teleporter";

        var dts = teleporter.GetComponent<DungeonTeleporterScript>();
        dts.teleport += Teleport;
        dts.player = player;
    }

    void Teleport()
    {
        // todo clean this a bit
        Reset();

        expands = nRooms - 1;

        camera.transform.position = rooms[0].CameraPosition();

        player.transform.position = rooms[0].PlayerPosition();

        GenerateDungeon();
        MakeDungeon();
        StartCoroutine(StartDungeon());

        GetComponent<Tutorial>().MakeText(-8, 5, "Use the WASD keys to move");
    }

    void Reset()
    {
        ClearDungeon();
        rooms = new List<Room>();
        roomBlocks = new List<Transform>();
        dungeonRooms = new List<Transform>();
        globalDoorLocations = new List<Door>();
        expandableRooms = new List<int>();

        MakeRoom(0, 0, baseRoomSize, 0); // base room

        expandableRooms.Add(0); // base room is expandable
    }

    IEnumerator StartDungeon()
    {
        yield return new WaitForSeconds(0.01f);
        Debug.Log("starting dungeon");

        if (started) yield break;
        started = true;
        foreach (Transform r in dungeonRooms)
        {
            r.GetComponent<DungeonRoomScript>().StartRoom();
        }

        dungeonRooms[0].GetComponent<DungeonRoomScript>().ShowRoom(true); // show doors
        yield break;
    }

    void ClearDungeon()
    {
        // foreach (Transform block in roomBlocks)
        // {
        //     Destroy(block.gameObject);
        // }
        foreach (Transform room in dungeonRooms)
        {
            Destroy(room.gameObject);
        }
        foreach (Door door in globalDoorLocations)
        {
            Destroy(door.doorTransform.gameObject);
        }

        if (teleporter != null)
        {
            Destroy(teleporter.gameObject);
        }

        // todo destroy old teleporter
        roomBlocks.Clear();
        started = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
