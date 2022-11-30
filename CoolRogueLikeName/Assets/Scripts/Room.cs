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