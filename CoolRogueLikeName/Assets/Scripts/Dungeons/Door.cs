using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door
{
    public float x;
    public float y;
    public int room1;
    public int room2;
    public DungeonRoomScript from;
    public DungeonRoomScript to;
    public Transform doorTransform;

    public Door(int x, int y, int room1, int room2, Wall onWall)
    {
        this.x = x; // - 0.5f;
        this.y = y; // - 0.5f;

        switch (onWall) {
            case Wall.North: case Wall.South:
                this.x += 0.5f;
                break;
            case Wall.East: case Wall.West:
                this.y += 0.5f;
                break;
        }

        this.room1 = room1;
        this.room2 = room2;
    }

    public void DoorVisibleDelegate(bool visible, int roomId)
    {
        if (roomId != room1 && roomId != room2)
        {
            return;
        }

        doorTransform.GetComponent<Renderer>().enabled = visible;
    }

    public void Unlock(int roomId)
    {
        if (roomId != room1 && roomId != room2)
        {
            return;
        }
        doorTransform.GetComponent<DungeonDoorScript>().Unlock();
    }
}
