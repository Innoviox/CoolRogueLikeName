using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int x;
    public int y;
    public int size;
    public int id;

    public List<int> expandableWalls;

    public List<Transform> doors;

    public Room(int x, int y, int size, int id)
    {
        this.x = x;
        this.y = y;
        this.size = size;
        this.id = id;

        expandableWalls = new List<int>();
        expandableWalls.Add(0); // north wall
        expandableWalls.Add(1); // east wall
        expandableWalls.Add(2); // south wall
        expandableWalls.Add(3); // west wall

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
                    if (door.position.x == i && door.position.y == j)
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
                    block = GameObject.Instantiate(prefab, new Vector3(i, j, 0), Quaternion.identity);
                    block.name = $"Wall ({i}, {j})";
                }
                else
                {
                    Transform prefab = blocksDict["Floor"];
                    block = GameObject.Instantiate(prefab, new Vector3(i, j, 0), Quaternion.identity);
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
        t.transform.localPosition += new Vector3(x - size, y + size, 0);
        blocks.Add(text.transform);

        // Debug.Log($"Made Rom {id} Size {size} Blocks {blocks.Count}");

        return blocks;
    }

    public int SharedWall(Room o)
    {
        if (y + size == o.y - o.size) return 0;
        if (x + size == o.x - o.size) return 1;
        if (y - size == o.y + o.size) return 2;
        if (x - size == o.x + o.size) return 3;
        return -1;
    }

    public Vector2 GenerateDoorLocation(int wall, Room other)
    {
        // todo should this method guarantee padding?
        int x = 0;
        int y = 0;
        switch (wall)
        {
            case 0:
                int xMin = Mathf.Max(this.x - this.size, other.x - other.size);
                int xMax = Mathf.Min(this.x + this.size, other.x + other.size);
                x = Random.Range(xMin, xMax);
                y = this.y + this.size;
                break;
            case 1:
                int yMin = Mathf.Max(this.y - this.size, other.y - other.size);
                int yMax = Mathf.Min(this.y + this.size, other.y + other.size);
                x = this.x + this.size;
                y = Random.Range(yMin, yMax);
                break;
            case 2:
                xMin = Mathf.Max(this.x - this.size, other.x - other.size);
                xMax = Mathf.Min(this.x + this.size, other.x + other.size);
                x = Random.Range(xMin, xMax);
                y = this.y - this.size;
                break;
            case 3:
                yMin = Mathf.Max(this.y - this.size, other.y - other.size);
                yMax = Mathf.Min(this.y + this.size, other.y + other.size);
                x = this.x - this.size;
                y = Random.Range(yMin, yMax);
                break;
        }

        return new Vector2(x, y);
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
        Transform door = GameObject.Instantiate(prefab, new Vector3(doorLocation.x, doorLocation.y, 0), Quaternion.identity);
        door.name = $"Door ({doorLocation.x}, {doorLocation.y})";
        doors.Add(door);
    }
}