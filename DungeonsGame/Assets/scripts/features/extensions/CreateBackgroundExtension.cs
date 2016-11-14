
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using SQLite4Unity3d;
using UniRx;
using UnityEngine;
using Entitas;

public static class CreateBackgroundExtension
{
    public static List<Tables.Background> Read(int floor, SQLiteConnection db)
    {
        //读取关卡数据
        var query = string.Format("SELECT name FROM sqlite_master WHERE type='table' AND name='{0}'",
            Res.configs.levels.ToString());
        var table = db.Query<TableName>(query, null);
        if (table.Count == 1)
            Debug.Log("level is not null");
        else
            Debug.Log("level is null");
        return null;
    }

    public static void CreateBackground(List<Tables.Background> backgroundData,Pools pools,int floor,string map)
    {
        var grids = new Grids[Res.columns, Res.rows];
        //创建关卡
        if (backgroundData == null)
            buildBackround(pools,floor,map, grids);
        else
            loadBackground(pools, floor, map);
    }

    private static void buildBackround(Pools pools,int floor,string map, Grids[,] grids)
    {
        var db = pools.input.runtimeData.db;
        var configLevel = db.Table<ConfigLevel>();
        var roomList = new List<ConfigLevel>();
        configLevel.ToObservable()
            .Where(x => x.Level == floor)
            .Do(x =>
            {
                int num = UnityEngine.Random.Range(x.MinCount, x.MaxCount);
                Observable.Range(0, num).Do(y =>
                {
                    var tmp = new ConfigLevel();
                    tmp.RoomName = x.RoomName;
                    tmp.Width = x.Width;
                    tmp.Height = x.Height;
                    roomList.Add(tmp);
                }).Subscribe();
            })
            .Subscribe();

        roomList = roomList.RandomSortList<ConfigLevel>();

        var positionList = null ?? new List<Vector2>();
        positionList.Clear();
        var roomCount = 1;
        var dir = new Vector2(0, 0);
        var doorPos = new Vector2(-1, -1);
        foreach (var room in roomList)
        {
            CreateRoom(ref dir,ref doorPos,pools,room,positionList,roomCount,map, grids);
            if (dir.x != 0 || dir.y != 0)
            {
                if (doorPos.x == -1)
                    throw new Exception("CreateBackgroundExtension buildBackround doorPos is wrong!");
                CreateAisle(dir,doorPos,grids,room,pools,map);
            }
            roomCount++;
        }
        roomList.Clear();
    }

    static void CreateAisle(Vector2 dir, Vector2 doorPos, Grids[,] grids, ConfigLevel room, Pools pools, string map)
    {
        var top = new Vector2(0, 1);
        var bottom = new Vector2(0, -1);
        var left = new Vector2(-1, 0);
        var right = new Vector2(1, 0);

        SetTile(grids, doorPos, pools, map);
        if (dir.y == 1)
        {
            SetTile(grids, doorPos + right, pools, map, 22);
            SetTile(grids, doorPos + left, pools, map, 24);

            SetTile(grids, doorPos + bottom, pools, map);
            SetTile(grids, doorPos + bottom + right, pools, map, 14);
            SetTile(grids, doorPos + bottom + left, pools, map, 16);

            SetTile(grids, doorPos + top, pools, map);
            SetTile(grids, doorPos + top + right, pools, map, 22);
            SetTile(grids, doorPos + top + left, pools, map, 24);
        }
        if (dir.y == -1)
        {
            SetTile(grids, doorPos + right, pools, map, 22);
            SetTile(grids, doorPos + left, pools, map, 24);

            SetTile(grids, doorPos + bottom, pools, map);
            SetTile(grids, doorPos + bottom + right, pools, map, 22);
            SetTile(grids, doorPos + bottom + left, pools, map, 24);

            SetTile(grids, doorPos + bottom+ bottom, pools, map);
            SetTile(grids, doorPos + bottom+ bottom + right, pools, map, 14);
            SetTile(grids, doorPos + bottom+ bottom + left, pools, map, 16);
        }
        if (dir.x == 1)
        {
            SetTile(grids, doorPos + top, pools, map, 10);
            SetTile(grids, doorPos + top + top, pools, map, 20);
            SetTile(grids, doorPos + bottom, pools, map, 21);

            SetTile(grids, doorPos + right, pools, map);
            SetTile(grids, doorPos + top + right, pools, map, 10);
            SetTile(grids, doorPos + top + top + right, pools, map, 20);
            SetTile(grids, doorPos + bottom + right, pools, map, 21);
        }
        if (dir.x == -1)
        {
            SetTile(grids, doorPos + top, pools, map, 10);
            SetTile(grids, doorPos + top + top, pools, map, 20);
            SetTile(grids, doorPos + bottom, pools, map, 21);

            SetTile(grids, doorPos + left, pools, map);
            SetTile(grids, doorPos + top + left, pools, map, 10);
            SetTile(grids, doorPos + top + top + left, pools, map, 20);
            SetTile(grids, doorPos + bottom + left, pools, map, 21);
        }
    }

    static void SetTile(Grids[,] grids,Vector2 pos,Pools pools,string map,int sp=0)
    {
        var grid = grids[(int) pos.x, (int) pos.y];
        if (sp == 0)
        {
            sp = UnityEngine.Random.Range(0, 7);
            grid.Tiletype = TileType.floor;
        }
        var go = GetGameObject(grid.RoomId, grid.RoomName, pos);
        go.GetComponent<SpriteRenderer>().sprite = pools.input.spriteList.sprites[map][sp];
    }

    private static void CreateRoom(ref Vector2 dir,ref Vector2 doorPos,Pools pools,ConfigLevel room,List<Vector2> pList,int roomCount,string sprite, Grids[,] grids)
    {
        //
        GameObject go = new GameObject(roomCount + "_" + room.RoomName);
        go.transform.SetParent(pools.input.holder.poolDic[Res.InPools.Board]);

        int x=-1, y=-1;
        int count=0;
        do
        {
            SelectDoorPoint(ref x, ref y, ref dir, pList, grids);
            doorPos = new Vector2(x, y);
            SetOriginPoint(ref x, ref y, dir, room);
            count++;
        } while (!CheckArea(x, y,room, grids) && count<5000);

        if (x == -1||count==5000)
            throw new Exception("CreateBackgroundExtension SetOriginPoint is wrong!");

        //var range = String.Format("range:{0},{1},{2},{3}", x, y, room.Width + x, room.Height + y);
        //range.print();
        //dir.print();

        if (dir.y == 1)
            y -= 2;

        TileMap map = TmxLoader.Parse(Res.RoomsPath + room.RoomName);
        foreach (var layer in map.Layers)
        {
            BuildLayer(layer, room, x, y, pList, go, roomCount, pools, sprite, grids,dir);
        }

    }

    static void BuildLayer(TiledLayer layer, ConfigLevel room, int x, int y, List<Vector2> pList, GameObject go,int roomCount,Pools pools,string sprite, Grids[,] grids,Vector2 dir)
    {
        for (int row = 0; row < layer.Height; row++)
        {
            for (int col = 0; col < layer.Width; col++)
            {
                var tileType = grids[x + col, y + room.Height - row];
                if (tileType.Tiletype == TileType.nul)
                    tileType.Tiletype = TileType.empty;

                int gid = layer.Data[col + row*layer.Width];
                if (gid != 0)
                {
                    GameObject tile = AddTile(layer, row, col, gid, room, x, y, roomCount, pools, sprite, dir, grids);
                    if (tile != null)
                    {
                        tile.transform.SetParent(go.transform);
                        var xx = (int) tile.transform.position.x;
                        var yy = (int) tile.transform.position.y;
                        grids[xx, yy].Tiletype = (TileType) Enum.Parse(typeof (TileType), layer.Name);
                        grids[xx, yy].RoomId = roomCount;
                        grids[xx, yy].RoomName = room.RoomName;

                        if (grids[xx, yy].Tiletype == TileType.roof)
                            pList.Add(new Vector2(xx, yy));
                    }

                }

            }
        }
    }

    static GameObject AddTile(TiledLayer layer, int row, int col, int gid, ConfigLevel room, int x, int y,int roomcount,Pools pools,string sprite,Vector2 dir,Grids[,] grids)
    {
        var pos = new Vector2(x + col, y + room.Height - row);
        var grid = grids[(int) pos.x, (int) pos.y];

        if (dir.y == 1 && grid.Tiletype != TileType.nul && grid.Tiletype != TileType.empty)
            return null;

        if (dir.y == -1 && grid.Tiletype != TileType.nul && grid.Tiletype != TileType.empty)
        {
            GameObject tmpGo = GetGameObject(grid.RoomId, grid.RoomName, pos);
            if (tmpGo == null)
                throw new Exception("CreateBackgroundExtension AddTile is wrong!");
            else
                GameObject.Destroy(tmpGo);
        }

        GameObject go = new GameObject(string.Format("{0},{1},{2}", roomcount, pos.x, pos.y), typeof(SpriteRenderer));
        go.AddComponent<BoxCollider2D>();
        go.transform.position = pos;

        //建立精灵
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.sprite = pools.input.spriteList.sprites[sprite][gid - 1];
        return go;
    }

    static bool CheckArea(int x, int y, ConfigLevel room,Grids[,] grids)
    {
        int dis = 2;
        if (x < dis || y < dis || x + room.Width > Res.columns - dis || y + room.Height > Res.rows - dis)
            return false;

        if (x < 0 || y < 0)
            throw new Exception("CheckArea is wrong!");

        for (int i = x; i < room.Width+x; i++)
        {
            for (int j = y; j < room.Height+y; j++)
            {
                if (grids[i, j].Tiletype != TileType.nul)
                    return false;
            }
        }
        return true;
    }

    static void SetOriginPoint(ref int x,ref int y, Vector2 dir, ConfigLevel room)
    {
        int fw = room.Width;
        int fh = room.Height;
        if (dir.y == -1 || dir.y == 1)
        {
            x -= Mathf.CeilToInt(fw*0.5f);
            if (dir.y == 1)
                y += 1;
            else
                y -= fh + 1;
        }
        else
        {
            y -= Mathf.CeilToInt(fh*0.5f);
            if (dir.x == 1)
                x += 1;
            else
                x -= fw;
        }

    }

    static void SelectDoorPoint(ref int x,ref int y,ref Vector2 dir,List<Vector2> pList,Grids[,] grids)
    {
        dir = new Vector2(0, 0);
        if (pList.Count == 0)
        {
            x = Res.columns/2;
            y = Res.rows/2;
        }
        else
        {
            int px, py;
            do
            {
                var num = UnityEngine.Random.Range(0, pList.Count);
                var pos = pList[num];

                px = (int)pos.x;
                py = (int)pos.y;
                TileType top = grids[px, py + 1].Tiletype;
                TileType bottom = grids[px, py - 1].Tiletype;
                TileType left = grids[px - 1, py].Tiletype;
                TileType right = grids[px + 1, py].Tiletype;
                bool leftRight = left == TileType.roof && right == TileType.roof;
                bool topBottom = top == TileType.roof && bottom == TileType.roof;

                if (top == TileType.nul && leftRight)
                    dir = new Vector2(0, 1);
                else if (bottom == TileType.wall_out && leftRight)
                    dir = new Vector2(0, -1);
                else if (left == TileType.nul && topBottom)
                    dir = new Vector2(-1, 0);
                else if (right == TileType.nul && topBottom)
                    dir = new Vector2(1, 0);
            } while (dir == new Vector2(0, 0) );
            x = px;
            y = py;
        }
    }

    static GameObject GetGameObject(int id, string name, Vector2 pos)
    {
        var goName = String.Format("{0} Views/{1}_{2}/{1},{3},{4}", Res.InPools.Board, id, name, pos.x, pos.y);
        GameObject tmpGo = GameObject.Find(goName);
        return tmpGo;
    }

    private static void loadBackground(Pools pools,int floor,string map)
    {
        //
    }

    public class TableName { public string Name { get; private set; } }

    public class ConfigLevel
    {
        public int ID { get; set; }
        public int Level { get; set; }
        public string RoomName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }

    }


}
