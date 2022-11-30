using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Wall // numbered clockwise, -1 = invalid
{
    None = -1,
    North = 0,
    East = 1,
    South = 2,
    West = 3,
}

namespace WallMethods
{
    public static class MyExtensions
    {
        public static Wall Opposite(this Wall wall)
        {
            return (Wall)(((int)wall + 2) % 4);
        }

        public static Wall UnGuaranteeable(this Wall wall)
        {
            return (Wall)(((int)wall + 1) % 4);
        }
    }
}