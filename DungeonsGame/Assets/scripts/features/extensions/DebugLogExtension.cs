
using System;
using UnityEngine;

public static class DebugLogExtension
{
    public static void print(this string ss)
    {
        Debug.Log(ss);
    }

    public static void print(this int ii)
    {
        print(ii.ToString());
    }

    public static void print(this Vector2 vec2)
    {
        var s = String.Format("vec2,x:{0},y:{1}", vec2.x, vec2.y);
        s.print();
    }

    public static void print(this Vector3 vec3)
    {
        var s = String.Format("vec3,x:{0},y:{1},z:{2}", vec3.x, vec3.y, vec3.z);
        s.print();
    }
}

