using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Room
{
    public int x;
    public int y;
    public int size;
    public int id;
    public List<Wall> expandableWalls;
    public List<Transform> doors;
    public List<Door> doorLocations;
    public int doorWidth = 1; // todo make a class for variables or smth
    public int bossSize = 15; // yeah we need a variables class
    public bool isBossRoom;
    private List<Transform> blocks;

    public Room(int x, int y, int size, int id)
    {
        Debug.Log(size);
        this.x = x;
        this.y = y;
        this.size = size;
        this.id = id;

        expandableWalls = new List<Wall>();
        expandableWalls.Add(Wall.North); // north wall
        expandableWalls.Add(Wall.East); // east wall
        expandableWalls.Add(Wall.South); // south wall
        expandableWalls.Add(Wall.West); // west wall

        doors = new List<Transform>();
        blocks = new List<Transform>();
        doorLocations = new List<Door>();

        isBossRoom = size == bossSize;
    }

    public bool InRoom(int x, int y)
    {
        if (x > this.x - this.size && x < this.x + this.size)
        {
            if (y > this.y - this.size && y < this.y + this.size)
            {
                return true;
            }
        }

        return false;
    }

    public bool Overlaps(int l2x, int l2y, int r2x, int r2y)
    {
        // from https://www.geeksforgeeks.org/find-two-rectangles-overlap/amp/
        int l1x = x - size;
        int l1y = y + size;
        int r1x = x + size;
        int r1y = y - size;

        // If one rectangle is on left side of other
        if (l1x >= r2x || l2x >= r1x)
            return false;

        // If one rectangle is above other
        if (r1y >= l2y || r2y >= l1y)
            return false;

        return true;
    }

    public Transform MakeRoom(Dictionary<string, Transform> blocksDict, Transform player, Camera camera)
    {
        var dungeonRoom = GameObject.Instantiate(blocksDict["DungeonRoom"], new Vector3(x, 0, y), Quaternion.identity);
        dungeonRoom.name = $"Room {id}";

        var drs = dungeonRoom.GetComponent<DungeonRoomScript>();
        drs.room = this;
        drs.blocksDict = blocksDict;
        drs.player = player;
        drs.camera = camera;
        drs.generateEnemies = id == 0; // only first room is shown at first
        drs.nEnemiesBase = id == 0 ? 0 : 2;

        return dungeonRoom;
    }

    public Wall SharedWall(Room o)
    {
        Wall w = Wall.None;
        if (y + size == o.y - o.size) w = Wall.North;
        if (x + size == o.x - o.size) w = Wall.East;
        if (y - size == o.y + o.size) w = Wall.South;
        if (x - size == o.x + o.size) w = Wall.West;

        var ov = Overlap(w, o);
        if (ov.max - ov.min > doorWidth * 2) return w; // todo maybe this can help with padding

        return Wall.None;
    }

    public Vector2 GenerateDoorLocation(Wall wall, Room other)
    {

        int x = 0;
        int y = 0;
        var overlap = Overlap(wall, other);
        int pos = Random.Range(overlap.min + doorWidth, overlap.max - doorWidth);

        switch (wall)
        {
            case Wall.North:
                x = pos;
                y = this.y + this.size;
                // ydelta = -1;
                break;
            case Wall.East:
                x = this.x + this.size;
                y = pos;
                // xdelta = 1;
                break;
            case Wall.South:
                x = pos;
                y = this.y - this.size;
                // ydelta = 1;
                break;
            case Wall.West:
                x = this.x - this.size;
                y = pos;
                // xdelta = -1;
                break;
        }

        return new Vector2(x, y); //, new Vector2(x + xdelta, y + ydelta) };
    }

    private (int min, int max) Overlap(Wall wall, Room other)
    {
        int min = 0, max = 0;
        switch (wall)
        {
            case Wall.North:
                min = Mathf.Max(this.x - this.size, other.x - other.size);
                max = Mathf.Min(this.x + this.size, other.x + other.size);
                break;
            case Wall.East:
                min = Mathf.Max(this.y - this.size, other.y - other.size);
                max = Mathf.Min(this.y + this.size, other.y + other.size);
                break;
            case Wall.South:
                min = Mathf.Max(this.x - this.size, other.x - other.size);
                max = Mathf.Min(this.x + this.size, other.x + other.size);
                break;
            case Wall.West:
                min = Mathf.Max(this.y - this.size, other.y - other.size);
                max = Mathf.Min(this.y + this.size, other.y + other.size);
                break;
        }
        return (min: min, max: max);
    }

    public void ClearDoors()
    {
        foreach (Transform door in doors)
        {
            GameObject.Destroy(door.gameObject);
        }
        doors.Clear();
    }

    public void AddDoor(Door doorLocation)
    {
        doorLocations.Add(doorLocation);
    }

    public Vector3 CameraPosition()
    {
        return new Vector3(x, 10, y - size - 4); // formula should be tweaked but its probably fine for now
    }

    public Vector3 PlayerPosition()
    {
        // Debug.Log($"PlayerPosition {x} {y} {size}");
        return new Vector3(x, 2, y);
    }

    public Bounds GetBounds() // if were gonna call this multiple times make a variable
    {
        var bounds = new Bounds(new Vector3(x, 0, y), new Vector3(size * 2, 10, size * 2));

        return bounds;
    }

    public void MakeRoomBlocks(DungeonRoomScript drs)
    {
        blocks.Clear();
        for (int i = x - size; i < x + size; i++)
        {
            for (int j = y - size; j < y + size; j++)
            {
                bool isDoor = false;
                Door d = null;
                foreach (Door door in doorLocations)
                {
                    if (door.x == i && door.y == j)
                    {
                        isDoor = true;
                        d = door;
                        break;
                    }
                }

                if (isDoor)
                {
                    // Transform prefab = drs.blocksDict["Door"];
                    // Transform door = GameObject.Instantiate(prefab, new Vector3(i, 0, j), Quaternion.identity);
                    // door.name = $"Door ({i}, {j})";
                    // door.transform.parent = drs.Parent();

                    // drs.UpdateDoorScript(door.GetComponent<DungeonDoorScript>(), d);

                    // doors.Add(door);
                }
                else
                {
                    Transform block;
                    if (i == x - size || i == x + size - 1 || j == y - size || j == y + size - 1)
                    {
                        // Transform prefab = drs.blocksDict["Wall"];
                        Transform prefab = drs.blocksDict["Floor"]; // for now don't generate walls
                        block = GameObject.Instantiate(prefab, new Vector3(i, 0, j), Quaternion.identity);
                        block.name = $"Wall ({i}, {j})";
                    }
                    else
                    {
                        Transform prefab = drs.blocksDict["Floor"];
                        block = GameObject.Instantiate(prefab, new Vector3(i, 0, j), Quaternion.identity);
                        block.name = $"Floor ({i}, {j})";
                    }

                    block.transform.parent = drs.Parent();
                    blocks.Add(block);
                }
            }
        }
    }

    public Vector2 RandomLocation()
    {
        int x = Random.Range(this.x - size + 2, this.x + size - 2);
        int y = Random.Range(this.y - size + 2, this.y + size - 2);
        return new Vector2(x, y);
    }

    public Vector3 RandomLocation(float y)
    {
        Vector2 loc = RandomLocation();
        return new Vector3(loc.x, y, loc.y);
    }

    public Vector3 Center(float yPos)
    {
        return new Vector3(x, yPos, y);
    }
}