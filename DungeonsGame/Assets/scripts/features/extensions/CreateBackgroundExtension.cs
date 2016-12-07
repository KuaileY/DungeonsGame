
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using SQLite4Unity3d;
using UniRx;
using UnityEngine;
using Entitas;

public static class CreateBackgroundExtension
{
    public static void CreateBackground(Pools pools,int floor,ref XElement[] board,ref GameObject[,] bgHolder)
    {
        var grids = new Grids[Res.columns, Res.rows];
        //创建关卡
        buildBackround(pools,floor, grids,board, bgHolder);
    }

    private static void buildBackround(Pools pools, int floor, Grids[,] grids, XElement[] XFloor, GameObject[,] bgHolder)
    {
        var db = pools.input.runtimeData.db;
        var configLevel = db.Table<ConfigLevel>();
        var roomList = null ?? new List<ConfigLevel>();
        var specialRoomList = null ?? new List<ConfigLevel>();
        specialRoomList.Clear();
        roomList.Clear();

        var num = 0;
        var prob = new List<int>();
        var tempList = new List<ConfigLevel>();
        var count = 0;
        configLevel.ToObservable()
            .Where(x => x.Level == floor)
            .Do(x =>
            {
                if (x.Probability >=100)
                {
                    num += x.Probability;
                    prob.Add(num);
                }

                var tmp = new ConfigLevel();
                tmp.RoomName = x.RoomName;
                tmp.Type = x.Type;
                tmp.Width = x.Width;
                tmp.Height = x.Height;
                tmp.Image = x.Image;
                tmp.Probability = x.Probability;
                tempList.Add(tmp);

                count = x.TotalCount;
            })
            .Subscribe();

        //创建普通房间
        for (int i = 0; i < count; i++)
        {
            var key = UnityEngine.Random.Range(1, prob[prob.Count - 1]);
            var item = prob.Find(key);
            roomList.Add(tempList[item]);
        }

        //创建特殊房间
        var rand = UnityEngine.Random.Range(1, 4);
        tempList.ToObservable()
            .Where(x => x.Probability < 100)
            .Do(x =>
            {
                if (x.Type == Res.roomType.shop.ToString())
                    roomList.Add(x);
                if (x.Type == Res.roomType.chest.ToString() && rand == x.Probability)
                    roomList.Add(x);
                if (x.Type == Res.roomType.boss.ToString() && rand == x.Probability)
                    roomList.Add(x);
            })
            .Subscribe();


        var positionList = null ?? new List<Vector2>();
        positionList.Clear();
        var roomCount = 1;
        var dir = new Vector2(0, 0);
        var doorPos = new Vector2(-1, -1);
        var fovChar = new char[Res.columns, Res.rows];
        foreach (var room in roomList)
        {
            CreateRoom(ref dir, ref doorPos, pools, room, positionList, roomCount, grids, XFloor, bgHolder,fovChar);
            if (dir.x != 0 || dir.y != 0)
            {
                if (doorPos.x == -1)
                    throw new Exception("CreateBackgroundExtension buildBackround doorPos is wrong!");
                CreateAisle(dir, doorPos, grids, room, pools, XFloor,fovChar);
            }
            roomCount++;
        }
        var ss = string.Empty;
        for (int x = Res.columns-1; x >=0; x--)
        {
            ss += "\n";
            for (int y = 0; y < Res.rows; y++)
            {
                if (fovChar[y, x] == char.MinValue)
                    ss += Res.TileTypeChar[(int) TileType.nul];
                else
                    ss += fovChar[y, x];
            }
        }
        XFloor[2].Value = ss;

    }

    static void CreateAisle(Vector2 dir, Vector2 doorPos, Grids[,] grids, ConfigLevel room, Pools pools, XElement[] XFloor,char[,] fovchar)
    {
        var top = new Vector2(0, 1);
        var bottom = new Vector2(0, -1);
        var left = new Vector2(-1, 0);
        var right = new Vector2(1, 0);
        var map = room.Image;
        if (dir.y == 1)
        {
            SetTile(grids, XFloor,fovchar, doorPos, pools, map, 0, true);
            SetTile(grids, XFloor,fovchar, doorPos + right, pools, map, 22);
            SetTile(grids, XFloor,fovchar, doorPos + left, pools, map, 24);

            SetTile(grids, XFloor,fovchar, doorPos + bottom, pools, map);
            SetTile(grids, XFloor,fovchar, doorPos + bottom + right, pools, map, 14);
            SetTile(grids, XFloor,fovchar, doorPos + bottom + left, pools, map, 16);

            SetTile(grids, XFloor,fovchar, doorPos + top, pools, map);
            SetTile(grids, XFloor,fovchar, doorPos + top + right, pools, map, 22);
            SetTile(grids, XFloor,fovchar, doorPos + top + left, pools, map, 24);
        }
        if (dir.y == -1)
        {
            SetTile(grids, XFloor,fovchar, doorPos, pools, map);
            SetTile(grids, XFloor,fovchar, doorPos + right, pools, map, 22);
            SetTile(grids, XFloor,fovchar, doorPos + left, pools, map, 24);

            SetTile(grids, XFloor,fovchar, doorPos + bottom, pools, map, 0, true);
            SetTile(grids, XFloor,fovchar, doorPos + bottom + right, pools, map, 22);
            SetTile(grids, XFloor,fovchar, doorPos + bottom + left, pools, map, 24);

            SetTile(grids, XFloor,fovchar, doorPos + bottom+ bottom, pools, map);
            SetTile(grids, XFloor,fovchar, doorPos + bottom+ bottom + right, pools, map, 14);
            SetTile(grids, XFloor,fovchar, doorPos + bottom+ bottom + left, pools, map, 16);
        }
        if (dir.x == 1)
        {
            SetTile(grids, XFloor,fovchar, doorPos, pools, map,0,true);
            SetTile(grids, XFloor,fovchar, doorPos + top, pools, map, 10);
            SetTile(grids, XFloor,fovchar, doorPos + top + top, pools, map, 20);
            SetTile(grids, XFloor,fovchar, doorPos + bottom, pools, map, 21);

            SetTile(grids, XFloor,fovchar, doorPos + right, pools, map);
            SetTile(grids, XFloor,fovchar, doorPos + top + right, pools, map, 10);
            SetTile(grids, XFloor,fovchar, doorPos + top + top + right, pools, map, 20);
            SetTile(grids, XFloor,fovchar, doorPos + bottom + right, pools, map, 21);
        }
        if (dir.x == -1)
        {
            SetTile(grids, XFloor,fovchar, doorPos, pools, map, 0, true);
            SetTile(grids, XFloor,fovchar, doorPos + top, pools, map, 10);
            SetTile(grids, XFloor,fovchar, doorPos + top + top, pools, map, 20);
            SetTile(grids, XFloor,fovchar, doorPos + bottom, pools, map, 21);

            SetTile(grids, XFloor,fovchar, doorPos + left, pools, map);
            SetTile(grids, XFloor,fovchar, doorPos + top + left, pools, map, 10);
            SetTile(grids, XFloor,fovchar, doorPos + top + top + left, pools, map, 20);
            SetTile(grids, XFloor,fovchar, doorPos + bottom + left, pools, map, 21);
        }
    }

    static void SetTile(Grids[,] grids, XElement[] XFloor,char[,] fovchar,Vector2 pos,Pools pools,string map,int sp=0,bool doorPos=false)
    {
        var grid = grids[(int) pos.x, (int) pos.y];
        if (sp == 0)
        {
            sp = UnityEngine.Random.Range(0, 7);
            grid.Tiletype = TileType.floor;
            fovchar[(int) pos.x, (int) pos.y] = Res.TileTypeChar[(int) TileType.floor];
            XFloor[1].Elements().ToObservable()
                .Where(x => x.Attribute("id").Value.toInt() == grid.RoomId)
                .Do(x =>
                {
                    int xx = x.Attribute("x").Value.toInt();
                    int yy = x.Attribute("y").Value.toInt();

                    var s = String.Format(",{0}|{1},", pos.x - xx, pos.y - yy);
                    var newValue = x.Element("obstacleData").Value.Replace(s, ",");
                    x.Element("obstacleData").Value = newValue;

                    if (doorPos)
                    {
                        if (x.Element("door") == null)
                            x.Add(new XElement("door"));
                        x.Element("door").Add(String.Format("{0}|{1},", pos.x - xx, pos.y - yy));
                    }
                    else
                        x.Element("floorData").Add(String.Format("{0}|{1},", pos.x - xx, pos.y - yy));

                })
                .Subscribe();
        }
        var go = GetGameObject(grid.RoomId, grid.RoomHierarchy,grid.RoomName, pos);
        go.GetComponent<SpriteRenderer>().sprite = pools.input.spriteList.sprites[map][sp];
    }

    private static void CreateRoom(ref Vector2 dir, ref Vector2 doorPos, Pools pools, ConfigLevel room,
        List<Vector2> pList, int roomCount, Grids[,] grids, XElement[] XFloor, GameObject[,] bgHolder,char[,] fovChar)
    {
        //选择门的位置，和下一个房间起始位置
        int x = -1, y = -1;
        int count = 0;

        do
        {
            SelectDoorPoint(ref x, ref y, ref dir, pList, grids);
            doorPos = new Vector2(x, y);
            SetOriginPoint(ref x, ref y, dir, room);
            count++;
        } while (!CheckArea(x, y, room, grids) && count < 5000);
        //检查选择函数是否出错
        if (x == -1 || count == 5000)
            throw new Exception("CreateBackgroundExtension SetOriginPoint is wrong!");
        //上方的需要向下移动两格
        if (dir.y == 1)
            y -= 1;
        //当前层次
        int roomHierarchy;
        if (roomCount == 1)
            roomHierarchy = roomCount;
        else
            roomHierarchy = grids[(int) doorPos.x, (int) doorPos.y].RoomHierarchy + 1;
        //建立保持对象
        GameObject go = new GameObject(roomCount + "_" + roomHierarchy + "_" + room.RoomName);
        go.transform.SetParent(pools.input.holder.poolDic[Res.InPools.Board]);
        //建立背景输出数据
        XElement XRoom = new XElement("room" + roomCount);
        XRoom.Add(new XAttribute("id", roomCount));
        XRoom.Add(new XAttribute("name", room.RoomName));
        XRoom.Add(new XAttribute("x", x));
        XRoom.Add(new XAttribute("y", y));
        XRoom.Add(new XAttribute("width", room.Width));
        XRoom.Add(new XAttribute("height", room.Height));
        XRoom.Add(new XAttribute("image", room.Image));
        var roomData = new string[room.Width*room.Height];
        //建立交互输出数据
        var floorData = new List<string>();
        var obstacleData = new List<string>();
        var waterData = new List<string>();
        XFloor[0].Add(XRoom);
        //根据图层创建房间
        TileMap map = TmxLoader.Parse(Res.RoomsPath + room.RoomName);
        foreach (var layer in map.Layers)
        {
            BuildLayer(layer, room, x, y, pList, go, roomCount, pools, grids, dir, roomHierarchy, roomData, floorData,
                obstacleData, waterData, bgHolder, fovChar);
        }
        if (floorData.Count < 1)
            throw new Exception("floorData is null!");
        //建立交互输出
        var XInteractive = new XElement("room"+roomCount);
        XInteractive.Add(new XAttribute("id", roomCount));
        XInteractive.Add(new XAttribute("name", room.RoomName));
        XInteractive.Add(new XAttribute("x", x));
        XInteractive.Add(new XAttribute("y", y));
        XInteractive.Add(new XAttribute("width", room.Width));
        XInteractive.Add(new XAttribute("height", room.Height));
        XFloor[1].Add(XInteractive);
        foreach (var OG in map.ObjectGroups)
        {
            var objectGroup = new XElement("objectGroup");
            objectGroup.Add(new XAttribute("name", OG.Name));
            outObjectGroup(OG, objectGroup);
            XInteractive.Add(objectGroup);
        }
        XInteractive.Add(new XElement("floorData", floorData));
        XInteractive.Add(new XElement("obstacleData", obstacleData));
        XInteractive.Add(new XElement("waterData", waterData));

        XRoom.Add(new XElement("data", roomData));

    }

    static void outObjectGroup(TiledObjectGroup objectGroup,XElement parent)
    {
        objectGroup.Objects.ToObservable()
            .Do(x =>
            {
                var obj = new XElement("object");
                obj.Add(new XAttribute("name", x.Name));
                obj.Add(new XAttribute("x", x.X/Res.tileSetWidth));
                obj.Add(new XAttribute("y", x.Y/Res.tileSetWidth));
                if (x.Width == Res.tileSetWidth)
                    obj.Add(new XAttribute("type", Res.objectType.point));
                else
                {
                    obj.Add(new XAttribute("type", Res.objectType.rect));
                    obj.Add(new XAttribute("width", x.Width/Res.tileSetWidth));
                    obj.Add(new XAttribute("height", x.Height/Res.tileSetWidth));
                }
                parent.Add(obj);
            })
            .Subscribe();
    }

    static void BuildLayer(TiledLayer layer, ConfigLevel room, int x, int y, List<Vector2> pList, GameObject go,
        int roomCount, Pools pools, Grids[,] grids, Vector2 dir, int roomHierarchy, string[] roomData,
        List<string> floorData, List<string> obstacleData, List<string> waterData, GameObject[,] bgHolder,char[,] fovChar)
    {
        int i = 0;
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
                    var pos = new Vector2(x + col, y + room.Height - row);
                    GameObject tile = new GameObject(string.Format("{0}_{3},{1},{2}", roomCount, pos.x, pos.y, roomHierarchy), typeof(SpriteRenderer));
                    tile.transform.position = pos;
                    tile.AddComponent<BoxCollider2D>();
                    SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                    sr.sprite = pools.input.spriteList.sprites[room.Image][gid - 1];
                    tile.GetComponent<Renderer>().enabled = false;

                    if (tile != null)
                    {
                        tile.transform.SetParent(go.transform);
                        var xx = (int) tile.transform.position.x;
                        var yy = (int) tile.transform.position.y;
                        grids[xx, yy].Tiletype = (TileType) Enum.Parse(typeof (TileType), layer.Name);
                        grids[xx, yy].RoomId = roomCount;
                        grids[xx, yy].RoomHierarchy = roomHierarchy;
                        grids[xx, yy].RoomName = room.RoomName;
                        bgHolder[xx, yy] = tile;


                        if (grids[xx, yy].Tiletype == TileType.roof)
                            pList.Add(new Vector2(xx, yy));
                        //输出数据
                        roomData[i] = gid.ToString() + ',';
                        if (layer.Name == TileType.floor.ToString())
                        {
                            floorData.Add(String.Format("{0}|{1},", xx - x, yy - y));
                            fovChar[xx, yy] = Res.TileTypeChar[(int)TileType.floor];
                            tile.GetComponent<SpriteRenderer>().material =
                                Resources.Load<Material>("Material/dungeon/floor");
                        }
                        else if (layer.Name == TileType.water.ToString())
                        {
                            waterData.Add(String.Format("{0}|{1},", xx - x, yy - y));
                            fovChar[xx, yy] = Res.TileTypeChar[(int) TileType.water];
                            tile.GetComponent<SpriteRenderer>().material =
                                Resources.Load<Material>("Material/dungeon/water");
                        }
                        else if (layer.Name == TileType.wall.ToString())
                        {
                            obstacleData.Add(String.Format("{0}|{1},", xx - x, yy - y));
                            fovChar[xx, yy] = Res.TileTypeChar[(int)TileType.wall];
                            tile.GetComponent<SpriteRenderer>().material =
                                Resources.Load<Material>("Material/dungeon/wall");
                        }
                        else
                        {
                            obstacleData.Add(String.Format("{0}|{1},", xx - x, yy - y));
                            fovChar[xx, yy] = Res.TileTypeChar[(int) TileType.obstacle];
                            tile.GetComponent<SpriteRenderer>().material =
                                Resources.Load<Material>("Material/dungeon/wall");
                        }
                    }

                }
                i++;
            }
        }
    }

    static GameObject AddTile(TiledLayer layer, int row, int col, int gid, ConfigLevel room, int x, int y,int roomcount,Pools pools,Vector2 dir,Grids[,] grids,int roomHierarchy)
    {
        var pos = new Vector2(x + col, y + room.Height - row);

        GameObject go = new GameObject(string.Format("{0}_{3},{1},{2}", roomcount, pos.x, pos.y, roomHierarchy), typeof(SpriteRenderer));
        go.AddComponent<BoxCollider2D>();
        go.transform.position = pos;
        go.transform.position += new Vector3(0, 0, 1);
        go.GetComponent<Renderer>().enabled = false;

        //建立精灵
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.sprite = pools.input.spriteList.sprites[room.Image][gid - 1];
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
            for (int j = y; j <= room.Height+y; j++)
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
            if (room.RoomName.Substring(0, 1) == "L")
                x -= Mathf.CeilToInt(fw * 0.3f); 
            else if (room.RoomName.Substring(0, 1) == "R")
                x -= Mathf.CeilToInt(fw * 0.65f);
            else
                x -= Mathf.CeilToInt(fw*0.5f);

            if (dir.y == 1)
                y += 1;
            else
                y -= fh + 1;
        }
        else
        {
            if (room.RoomName.Substring(0, 1) == "L"|| room.RoomName.Substring(0, 1) == "R")
                y -= Mathf.CeilToInt(fw * 0.35f);
            else
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
                else if (bottom == TileType.nul && leftRight)
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

    static GameObject GetGameObject(int id,int hierarchy, string name, Vector2 pos)
    {
        var goName =
            String.Format("{0} Views/{1}_{5}_{2}/{1}_{5},{3},{4}", Res.InPools.Board, id, name, pos.x, pos.y, hierarchy);
        GameObject tmpGo = GameObject.Find(goName);
        return tmpGo;
    }


    public class TableName { public string Name { get; private set; } }

    public class ConfigLevel
    {
        public int Level { get; set; }
        public string Type { get; set; }
        public string RoomName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int TotalCount { get; set; }
        public int Probability { get; set; }
        public string Image { get; set; }
    }


}
