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

    // todo doors

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

    public bool CornersInSquare(int x1, int y1, int x2, int y2)
    {
        // todo make this better
        int cx1 = this.x - this.size;
        int cy1 = this.y + this.size;

        int cx2 = this.x + this.size;
        int cy2 = this.y - this.size;

        int cx3 = this.x - this.size;
        int cy3 = this.y - this.size;

        int cx4 = this.x + this.size;
        int cy4 = this.y + this.size;

        if (x1 <= cx1 && x2 >= cx1 && y1 >= cy1 && y2 <= cy1)
        {
            return true;
        }

        if (x1 <= cx2 && x2 >= cx2 && y1 >= cy2 && y2 <= cy2)
        {
            return true;
        }

        if (x1 <= cx3 && x2 >= cx3 && y1 >= cy3 && y2 <= cy3)
        {
            return true;
        }

        if (x1 <= cx4 && x2 >= cx4 && y1 >= cy4 && y2 <= cy4)
        {
            return true;
        }

        return false;
    }

    // public bool Overlaps(int x1, int y1, int x2, int y2)
    // {
    //     if (x - size > x2 || x1 > x + size)
    //     {
    //         return false;
    //     }

    //     if (y + size > y1 || y2 > y - size)
    //     {
    //         return false;
    //     }

    //     return true;
    // }

    public bool Overlaps(int l2x, int l2y, int r2x, int r2y)
    {
        int l1x = x - size;
        int l1y = y + size;
        int r1x = x + size;
        int r1y = y - size;

        // If one rectangle is on left side of other
        if (l1x > r2x || l2x > r1x)
            return false;

        // If one rectangle is above other
        if (r1y > l2y || r2y > l1y)
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
}