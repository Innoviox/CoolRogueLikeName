using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door
{
    public int x;
    public int y;
    int room1;
    int room2;
    public DungeonRoomScript from;
    public DungeonRoomScript to;
    public Transform doorTransform;

    public Door(int x, int y, int room1, int room2)
    {
        this.x = x;
        this.y = y;
        this.room1 = room1;
        this.room2 = room2;
    }
}
