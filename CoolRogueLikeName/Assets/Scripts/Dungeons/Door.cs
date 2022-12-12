using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door
{
    // x and y are the locations of the door
    public float x;
    public float y;
    // rooms this door goes between
    public int room1;
    public int room2;
    // scripts of the rooms this door goes between
    public DungeonRoomScript from;
    public DungeonRoomScript to;
    // transform
    public Transform doorTransform;
    // wall this door is on
    public Wall onWall;
    // if this door is a boss door
    public bool isBossDoor = false;

    public Door(int x, int y, int room1, int room2, Wall onWall)
    {
        this.x = x;
        this.y = y;
        this.onWall = onWall;
        this.room1 = room1;
        this.room2 = room2;
    }

    public void Show(bool visible, int roomId)
    {
        if (roomId != room1 && roomId != room2) // check if this door is in the room we are in
        {
            return;
        }

        doorTransform.GetComponent<Renderer>().enabled = visible;
    }

    public void Lock(bool lockDoors, int roomId)
    {
        if (roomId != room1 && roomId != room2) // check if this door is in the room we are in
        {
            return;
        }

        if (lockDoors)
        {
            doorTransform.GetComponent<DungeonDoorScript>().Lock();
        }
        else
        {
            doorTransform.GetComponent<DungeonDoorScript>().Unlock();
        }
    }
}
