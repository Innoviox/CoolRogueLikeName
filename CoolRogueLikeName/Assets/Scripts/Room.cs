using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int x;
    public int y;
    public int size;
    public int id;
    public List<Wall> expandableWalls;
    public List<Transform> doors;
    public int doorWidth = 1; // todo make a class for variables or smth

    public Room(int x, int y, int size, int id)
    {
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

    public List<Transform> MakeRoom(Dictionary<string, Transform> blocksDict)
    {
        List<Transform> blocks = new List<Transform>();
        for (int i = x - size; i < x + size; i++)
        {
            for (int j = y - size; j < y + size; j++)
            {
                bool isDoor = false;
                foreach (Transform door in doors)
                {
                    if (door.position.x == i && door.position.z == j)
                    {
                        isDoor = true;
                        break;
                    }
                }
                if (isDoor) continue;

                Transform block;
                if (i == x - size || i == x + size - 1 || j == y - size || j == y + size - 1)
                {
                    Transform prefab = blocksDict["Wall"];
                    block = GameObject.Instantiate(prefab, new Vector3(i, 0, j), Quaternion.identity);
                    block.name = $"Wall ({i}, {j})";
                }
                else
                {
                    Transform prefab = blocksDict["Floor"];
                    block = GameObject.Instantiate(prefab, new Vector3(i, 0, j), Quaternion.identity);
                    block.name = $"Floor ({i}, {j})";
                }

                blocks.Add(block);
            }
        }

        GameObject text = new GameObject();
        TextMesh t = text.AddComponent<TextMesh>();
        t.text = $"{id}";
        t.fontSize = 100;
        t.color = Color.black;
        t.transform.localPosition += new Vector3(x - size, 0, y + size);
        blocks.Add(text.transform);

        // Debug.Log($"Made Rom {id} Size {size} Blocks {blocks.Count}");

        return blocks;
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
        // todo should this method guarantee padding?
        int x = 0;
        int y = 0;
        var overlap = Overlap(wall, other);
        int pos = Random.Range(overlap.min + doorWidth, overlap.max - doorWidth);

        switch (wall)
        {
            case Wall.North:
                x = pos;
                y = this.y + this.size;
                break;
            case Wall.East:
                x = this.x + this.size;
                y = pos;
                break;
            case Wall.South:
                x = pos;
                y = this.y - this.size;
                break;
            case Wall.West:
                x = this.x - this.size;
                y = pos;
                break;
        }

        return new Vector2(x, y);
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

    public void AddDoor(Vector2 doorLocation, Dictionary<string, Transform> blocksDict)
    {
        Transform prefab = blocksDict["Door"];
        Transform door = GameObject.Instantiate(prefab, new Vector3(doorLocation.x, 0, doorLocation.y), Quaternion.identity);
        door.name = $"Door ({doorLocation.x}, {doorLocation.y})";
        doors.Add(door);
    }

    public Vector3 CameraPosition()
    {
        return new Vector3(x, 10, y - size - 5); // formula should be tweaked but its probably fine for now
    }

    public Vector3 PlayerPosition()
    {
        Debug.Log($"PlayerPosition {x} {y} {size}");
        return new Vector3(x, y, 2);
    }
}