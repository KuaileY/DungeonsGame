
using System;
using UnityEngine;

public static class InputExtension
{
    public static Vector2 getDir(Vector2 pos)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;
        //第一象限
        if (x >= 0 && y >= 0)
        {
            if (x >= y)
                return new Vector2(1, 0);
            else
                return new Vector2(0, 1);
        }
        //第三象限
        if (x <= 0 && y <= 0)
        {
            if (x >= y)
                return new Vector2(0, -1);
            else
                return new Vector2(-1, 0);
        }
        //第四象限
        if (x >= 0 && y < 0)
        {
            if (x >= Mathf.Abs(y))
                return new Vector2(1, 0);
            else
                return new Vector2(0, -1);
        }
        //第二象限
        if (x < 0 && y >= 0)
        {
            if (Mathf.Abs(x) >= y)
                return new Vector2(-1, 0);
            else
                return new Vector2(0, 1);
        }
        throw new Exception("InputExtension getDir is wrong!");
    }
}

