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
        if (x >= this.x - this.size && x <= this.x + this.size)
        {
            if (y >= this.y - this.size && y <= this.y + this.size)
            {
                return true;
            }
        }

        return false;
    }
}