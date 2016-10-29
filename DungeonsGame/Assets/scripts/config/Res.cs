
using System.Collections.Generic;
using UnityEngine;


#region Data----
//levelData---------------
public enum TileType
{
    empty, floor, wall_in, wall_out, door, corner, roof, water, stairDown, stairUp, Obstacle, Normal,
}
public struct SingleRoom
{
    public string name;
    public Vector2 dir;
    public int id;
    public int width;
    public int height;
    public Vector2 pos;
    public TileType[,] grid;
    public GameObject[,] tiles;
    public int[] data;
}
public struct SingleGrid
{
    public List<SingleRoom> rooms;
    public  Tile[,] grids;
    public string name;
    public int width;
    public int height;
    public struct Tile
    {
        public int roomID;
        public int roomX;
        public int roomY;
        public TileType type;
    }
}
public struct LevelData
{
    public static List<SingleGrid> grids;
}
//itemsData--------------
public struct SingleItem
{
    public int id;
    public int dungeonId;
    public Res.InPools inPool;
    public Dictionary<Component, Object> status;
}

public struct ItemsData
{
    public static HashSet<SingleItem> items;
}
#endregion

public static class Res
{
    public const string player = "GameElements/Player";
    public const string food = "GameElements/Food";
    public const string door = "Items/door_close";

    public const float moveTime = 0.3f;
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

    public enum Files
    {
        levelData,
        itemsData,
    }
    #endregion

    #region Board----
    public const int columns = 80;
    public const int rows       = 80;
    public const int roomCount = 3;
    public const string RoomsXml = "database/dungeon_rooms";
    public static string RoomsPath = "Tmxs/";


    public enum Rooms
    {
        BigRoom, BigRoom1, MiddleRoom, MiddleRoom1, SmallRoom,
    }

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
        Input
    }

}
