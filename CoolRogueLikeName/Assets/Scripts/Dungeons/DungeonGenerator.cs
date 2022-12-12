using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using WallMethods;
using TMPro;

public class DungeonGenerator : MonoBehaviour
{
    // set sizes for rooms
    public int baseRoomSize = 10;
    public int minRoomSize = 7;
    public int maxRoomSize = 12;
    public int doorSize = 1;
    public int bossSize = 15;
    public int nRooms = 10; // number of rooms to be generated (not including center room)
    public List<Transform> blocks; // id prefer this to be a dict but unity doesnt do dicts in the inspector
    private Dictionary<string, Transform> blocksDict;
    public new Camera camera;
    public Transform playerPrefab; // the player prefab to instantiate
    private Transform player; // the actual player
    private List<Transform> dungeonRooms; // the transform parents of the rooms
    private List<Door> globalDoorLocations; // the locations of the doors
    private bool started = false; // if the dungeon has started
    private RoomGenerator roomGenerator; // the room generator
    private Transform teleporter = null; // the teleporter
    public Transform tutorial; // the tutorial
    public Tutorial tutorialComp;
    private int dungeonN = 0; // how many dungeons have been generated
    private int roomN = 0; // how many rooms have been walked through
    private List<Vector2> locationsNextToDoors; // the doors were originally one wide but now they are two wide
    private List<Room> rooms; // all rooms
    private List<int> expandableRooms; // rooms that still have expandable walls
    // UI things
    public TMP_Text maxHealthText;
    public Slider healthSlider;
    public Slider weaponSlider;
    public Slider jumpSlider;
    public Slider dashSlider;
    public Slider bossSlider;
    public TMP_Text maxJumpsText;
    public TMP_Text maxDashText;

    // Start is called before the first frame update
    void Start()
    {
        tutorialComp = tutorial.GetComponent<Tutorial>();

        blocksDict = new Dictionary<string, Transform>();
        foreach (Transform block in blocks) // create blocks dictionary
        {
            blocksDict.Add(block.name, block);
        }

        // instantiate player
        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        player.parent = transform;

        // set up hud
        player.GetComponent<PlayerHealth>().SetHud(maxHealthText, healthSlider);
        player.GetComponent<PlayerWeaponHolder>().SetHud(weaponSlider);
        player.GetComponent<Movement>().SetHud(jumpSlider, dashSlider, maxJumpsText, maxDashText);

        // set up room generator
        roomGenerator = GetComponent<RoomGenerator>();
        roomGenerator.Setup();

        // set up lists
        rooms = new List<Room>();
        dungeonRooms = new List<Transform>();
        globalDoorLocations = new List<Door>();
        expandableRooms = new List<int>();
        locationsNextToDoors = new List<Vector2>();

        // teleport = make new dungeon
        Teleport();
    }

    Room MakeRoom(int x, int y, int size, int depth)
    {
        // make a new room, add it to rooms, and mark it expandable
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
        // pick a random room to expand
        int roomToExpand = expandableRooms[UnityEngine.Random.Range(0, expandableRooms.Count)];
        Room room = rooms[roomToExpand];

        // pick a wall on the room to expand
        Wall wallToExpand = room.expandableWalls[UnityEngine.Random.Range(0, room.expandableWalls.Count)];

        int tries = 0, maxTries = bossRoom ? 100 : 10; // we really wanna generate the boss room
        while (!ExpandWall(room, wallToExpand, bossRoom) && tries < maxTries)
        {
            // if we cant expand the wall, pick a new room and a new wall
            roomToExpand = expandableRooms[UnityEngine.Random.Range(0, expandableRooms.Count)];
            room = rooms[roomToExpand];
            wallToExpand = room.expandableWalls[UnityEngine.Random.Range(0, room.expandableWalls.Count)];
            tries++;
        }

        if (room.expandableWalls.Count == 0)
        {
            // remove room from expandable rooms if it has no more expandable walls
            expandableRooms.Remove(roomToExpand);
        }
    }

    bool ExpandWall(Room room, Wall wallToExpand, bool bossRoom)
    {
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

        return true;
    }

    int GetMaxSize(Room room, Wall wallToExpand, bool bossRoom)
    {
        // get the biggest size that a room could be generated along this wall
        int maxSize = (int)minRoomSize;
        int maximum = bossRoom ? bossSize : maxRoomSize;

        while (maxSize < maximum * 2)
        {
            maxSize++;

            if (InOtherRoom(room, wallToExpand, maxSize))
            { // if we've hit another room we are at the max size
                break;
            }
        }
        maxSize /= 2;

        if (maxSize < minRoomSize)
        {
            return 0;
        }

        return maxSize;
    }

    int GenerateRoomSize(int maxSize)
    {
        // random room size between min and maxSize
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
        // generate a random offset for the new room
        int max1 = newRoomSize + oldRoomSize - doorSize * 2;
        int max2 = 2 * maxSize - newRoomSize - oldRoomSize;
        return (int)UnityEngine.Random.Range(newRoomSize - oldRoomSize, Mathf.Min(max1, max2));
    }

    void GenerateDoors()
    {
        // generate all door locations
        var doors = new Dictionary<int, List<Wall>>(); // dict from room id to list of walls with doors
        foreach (Room room1 in rooms)
        {
            foreach (Room room2 in rooms)
            {
                if (room1.id == room2.id) continue;

                Wall wall = room1.SharedWall(room2); // get locaation on shared wall
                if (wall == Wall.None) continue;
                if (doors.ContainsKey(room2.id) && doors[room2.id].Contains(wall.Opposite())) continue;

                var locations = room1.GenerateDoorLocation(wall, room2); // returns door location & location next to door
                globalDoorLocations.Add(new Door((int)locations[0].x, (int)locations[0].y, room1.id, room2.id, wall));
                locationsNextToDoors.Add(locations[1]);

                // add to doors dict
                if (doors.ContainsKey(room1.id))
                {
                    doors[room1.id].Add(wall);
                }
                else
                {
                    doors[room1.id] = new List<Wall> { wall };
                }

                if (room1.isBossRoom || room2.isBossRoom)
                { // set up boss variables
                    room1.hasBossDoor = true;
                    room2.hasBossDoor = true;
                    globalDoorLocations[globalDoorLocations.Count - 1].isBossDoor = true;
                }
            }
        }

    }

    void GenerateDungeon()
    {
        try
        {
            ExpandN(nRooms - 1 + (dungeonN - 1) * 2); // add 2 rooms per dungeon level
            Expand(true); // expand the boss room
            GenerateDoors();
        }
        catch (Exception e) // sometimes there is an index out of range error, just retry
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
            // make the room & generate the blocks
            Transform drt = room.MakeRoom(blocksDict, player, camera);
            roomGenerator.GenerateRoom(drt, new Vector3(room.x, 0, room.y), room.size, room.size, globalDoorLocations, locationsNextToDoors);

            drt.parent = GetComponent<Transform>();

            // set up the doors
            foreach (Door door in globalDoorLocations)
            {
                drt.GetComponent<DungeonRoomScript>().AddDelegates(door);
            }

            // set up the boss HP slider
            if (room.isBossRoom)
            {
                drt.GetComponent<DungeonRoomScript>().SetHud(bossSlider);
            }

            dungeonRooms.Add(drt);
        }

        MakeDoors();
    }

    void MakeDoors()
    {
        foreach (Door door in globalDoorLocations)
        {
            // instantiate the door
            Transform prefab = door.isBossDoor ? blocksDict["BossDoor"] : blocksDict["Door"];
            Transform doorTransform = GameObject.Instantiate(prefab, new Vector3(door.x, 0.5f, door.y), Quaternion.identity);
            doorTransform.name = $"Door ({door.x}, {door.y})";
            doorTransform.parent = transform;

            // proper rotation
            switch (door.onWall)
            {
                case Wall.North:
                    doorTransform.Rotate(0, 90, 0);
                    break;
                case Wall.East:
                    doorTransform.Rotate(0, 180, 0);
                    break;
                case Wall.South:
                    doorTransform.Rotate(0, 270, 0);
                    break;
                case Wall.West:
                    doorTransform.Rotate(0, 0, 0);
                    break;
            }

            // set up door script
            var drs = doorTransform.GetComponent<DungeonDoorScript>();
            drs.player = player;
            drs.door = door;
            drs.roomThisDoorLeadsFrom = dungeonRooms[door.room1].GetComponent<DungeonRoomScript>();
            drs.roomThisDoorLeadsTo = dungeonRooms[door.room2].GetComponent<DungeonRoomScript>();

            if (!door.isBossDoor)
            {
                doorTransform.Rotate(0, 0, 90);
            }
            else
            {
                doorTransform.Rotate(0, 90, 0);
                doorTransform.position = new Vector3(door.x, 0, door.y);
            }

            door.doorTransform = doorTransform;
        }
    }

    public void MakeTeleporter()
    {
        Vector3 loc = rooms[rooms.Count - 1].Center(0.5f);
        Transform prefab = blocksDict["Teleporter"];
        teleporter = GameObject.Instantiate(prefab, loc, Quaternion.identity);
        teleporter.name = "Teleporter";

        var dts = teleporter.GetComponent<DungeonTeleporterScript>();
        dts.teleport += Teleport;
        dts.player = player;

        teleporter.position += new Vector3(5.0f, 0.0f, 0.0f);
    }

    void Teleport()
    {
        Reset();

        camera.transform.position = rooms[0].CameraPosition();
        player.transform.position = rooms[0].PlayerPosition();

        GenerateDungeon();
        MakeDungeon();

        dungeonN += 1;

        StartCoroutine(StartDungeon());
    }

    void Reset()
    {
        ClearDungeon();

        // clear all the lists
        rooms.Clear();
        dungeonRooms.Clear();
        globalDoorLocations.Clear();
        expandableRooms.Clear();
        locationsNextToDoors.Clear();

        MakeRoom(0, 0, baseRoomSize, 0); // base room

        expandableRooms.Add(0); // base room is expandable
        tutorialComp.ClearTutorial();

        bossSlider.gameObject.SetActive(false);
    }

    IEnumerator StartDungeon()
    {
        yield return new WaitForSeconds(0.01f); // wait for Start to get called

        if (started)
        {
            yield break;
        }
        started = true;

        foreach (Transform r in dungeonRooms)
        {
            r.GetComponent<DungeonRoomScript>().StartRoom(); // start all rooms
        }

        dungeonRooms[0].GetComponent<DungeonRoomScript>().ShowRoom(true); // show doors
        if (tutorialComp.unlockFirstRoom == 0)
        {
            dungeonRooms[0].GetComponent<DungeonRoomScript>().lockDoors(true, 0); // lock doors
        }

        tutorialComp.StartTutorial();
        GetComponent<MusicPlayer>().state = 1; // in-game music state
        yield break;
    }

    void ClearDungeon()
    {
        // delete blocks that make up the dungeon
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

        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialComp.unlockFirstRoom == 1)
        {
            dungeonRooms[0].GetComponent<DungeonRoomScript>().lockDoors(false, 0);
            tutorialComp.unlockFirstRoom = 2; // dont run again
        }
    }

    public int TickRoomN()
    {
        roomN += 1;
        return roomN;
    }

    public int GetDungeonN()
    {
        return dungeonN;
    }
}
