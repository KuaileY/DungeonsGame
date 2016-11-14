﻿
using System.Collections.Generic;
using UnityEngine;


#region Data----
//levelData---------------
public enum Direction
{
    center,north,east,south,west
}
public enum TileType
{
    nul,empty, floor, wall_in, wall_out, door, corner, roof, water, stairDown, stairUp, Obstacle, Normal
}

public struct Grids
{
    public int RoomId;
    public string RoomName;
    public TileType Tiletype;
}
#endregion

public static class Res
{

    #region prefabs----
    public static class Prefabs
    {
        public const string player = "GameElements/Player";
        public const string food = "GameElements/Food";
        public const string door = "Items/door_close";
    }
    #endregion

    public static string dataPath = Application.dataPath;
    public const float moveTime = 0.3f;

    #region configs----
    public static readonly string configPath = dataPath+ "/art/Resources/database/";
    public const string xlsxExtension = ".xlsx";
    public enum configs
    {
        levels,
    }

    public enum xlsType
    {
        title,
        data,
    }
    #endregion

    #region Save----
    public static readonly string PathURL =
#if UNITY_ANDROID   //安卓  
        "jar:file://" + Application.dataPath + "!/assets/";  
#elif UNITY_IPHONE  //iPhone  
        Application.dataPath + "/Raw/";  
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR  //windows平台和web平台  
        Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;  
#endif

    public const string dbName = "database.sqlite3";
    public enum Files
    {
        levelData,
        itemsData,
    }
    #endregion

    #region Board----
    public const int columns = 90;
    public const int rows       = 90;
    public static string RoomsPath = "Tmxs/";


    public static readonly char[] TileTypeChar =
    {
        '-',    //empty
        '.',    //floor
        '#',   //wall_in
        '#',    //wall_out
        '+',    //door
        '*',    //corner
        '*',    //roof
        '~',    //water
        '<',    //stairDown
        '>',    //stairUp
        '^',    //Obstacle
        '.',    //Normal
    };

    #endregion

    #region View----

    static string viewsPath                = "Prefabs/UI/Views/";
    public static readonly UIType GameStart = new UIType(viewsPath + "GameStartView");
    public static readonly UIType GameOver  = new UIType(viewsPath + "GameOverView");
    public static readonly UIType Options   = new UIType(viewsPath + "MainUIView");

    #endregion

    #region Maps----
    public static string PrefabPath = "Prefabs/";
    public static string mapsTexturePath = "Sprites/dungeon/";

    public static readonly string[] maps =
    {
        "dungeontiles-blue",
        "dungeontiles-dark",
        "dungeontiles-frost",
        "dungeontiles-light",
        "dungeontiles-poison",
        "dungeontiles-sand",
    };

    public enum SubMaps
    {
        //保持与Sprites/dungeon/dungeontiles.xml同步
        floor_1,
        floor_2,
        floor_3,
        floor_4,
        floor_5,
        floor_6,
        floor_damaged_1,
        floor_damaged_2,
        floor_damaged_3,
        floor_damaged_4,
        roof_bottom_left,
        roof_bottom_right,
        roof_bottom_tee,
        roof_fourway,
        roof_left_tee,
        roof_right_tee,
        roof_single,
        roof_single_bottom,
        roof_single_left,
        roof_single_middle,
        roof_single_right,
        roof_single_top,
        roof_top_left,
        roof_top_right,
        roof_vertical,
        stair_up,
        star_down,
        wall_1,
        wall_2,
        wall_damage_1,
        wall_damage_2,
        wall_left,
        wall_right,
        wall_single,
        water_1,
        water_2,
        water_bottom_left,
        water_bottom_middle,
        water_bottom_right,
        water_in_bottom_left,
        water_in_bottom_right,
        water_in_top_left,
        water_in_top_right,
        water_left,
        water_out_top_left,
        water_out_top_middle,
        water_out_top_right,
        water_right,
        water_single,
        water_single_left,
        water_single_middle,
        water_single_right,
    }
    #endregion

    public enum InPools
    {
        //对象池
        Board,
        Core,
        Input,
        Blueprints,
    }

}
