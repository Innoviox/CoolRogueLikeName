using System;

public class Room
{
    public int x;
    public int y;
    public int size;

    public List<int> expandableWalls;

    // todo doors

    public Room(int x, int y, int size)
    {
        this.x = x;
        this.y = y;
        this.size = size;

        expandableWalls = new List<int>();
        expandableWalls.Add(0); // north wall
        expandableWalls.Add(1); // east wall
        expandableWalls.Add(2); // south wall
        expandableWalls.Add(3); // west wall
    }
}