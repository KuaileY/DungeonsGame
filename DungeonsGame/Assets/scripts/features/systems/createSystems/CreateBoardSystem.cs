using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreateBoardSystem : IReactiveSystem, ISetPools
{
    public TriggerOnEvent trigger { get { return BoardMatcher.GameBoard.OnEntityAdded(); } }
    Pools _pools;
    //房间标识
    int _roomCount = 1;
    //门的位置
    Vector2 _doorPos;
    //方向
    Vector2 _dir = new Vector2(0, 0);
    //保持对象
    Transform _trans;
    //当前画布
    SingleGrid _grid;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }

    public void Execute(List<Entity> entities)
    {
        TestLoadConfig.log.Trace("CreateBoardSystem Execute");
        _trans = _pools.input.holder.poolDic[entities[0].pool.name];
        _grid = BoardExtension.createGrid(Res.columns, Res.rows, Res.maps[0]);
        LevelData.grids.Add(_grid);
        int floor = _pools.board.gameBoard.floor;
        createBoard(floor);
    }

    void createBoard(int floor)
    {
        TestLoadConfig.log.Trace("gameBoard floor:" + floor);
        createFirstRoom(randomName());
        createAfterRooms();
    }

    void createAfterRooms()
    {
        while (_roomCount <= Res.roomCount)
            createFeature(randomName());
    }

    void createFeature(string roomName)
    {
        int roomID = -1;
        string name;
        int fw = 0, fh = 0;
        Vector2 pos = new Vector2(0, 0);
        do
        {
            selectPoint(ref roomID);
        } while (!setPoint(out name, ref pos, ref fw, ref fh));

        int x = (int)pos.x;
        int y = (int)pos.y;
        if (checkArea(x, y, x + fw, y + fh))
        {
            if (_dir.y==1)
            {
                pos -= new Vector2(0, 2);
            }
            //创建房间
            createRoom(name, pos);
            //建立过道
            createAisle();
        }
    }

    void createAisle()
    {
        int tx = (int)_doorPos.x;
        int ty = (int)_doorPos.y;
        var grid = _grid.grids[tx, ty];
        //grid.type = TileType.door;
        var _rooms = _grid.rooms;
        if (_dir.y == 1)
        {
            var nGrid = _grid.grids[tx, ty + 1];
            setTile(grid.roomID, grid.roomX, grid.roomY, _rooms, 0, TileType.door);
            setTile(grid.roomID, grid.roomX + 1, grid.roomY, _rooms, 22);
            setTile(grid.roomID, grid.roomX - 1, grid.roomY, _rooms, 24);

            setTile(grid.roomID, grid.roomX, grid.roomY + 1, _rooms);
            setTile(grid.roomID, grid.roomX + 1, grid.roomY + 1, _rooms, 14);
            setTile(grid.roomID, grid.roomX - 1, grid.roomY + 1, _rooms, 16);

            setTile(nGrid.roomID, nGrid.roomX, nGrid.roomY, _rooms);
            setTile(nGrid.roomID, nGrid.roomX + 1, nGrid.roomY, _rooms, 22);
            setTile(nGrid.roomID, nGrid.roomX - 1, nGrid.roomY, _rooms, 24);
        }
        if (_dir.y == -1)
        {
            var sGrid = _grid.grids[tx, ty - 2];
            setTile(grid.roomID, grid.roomX, grid.roomY, _rooms, 0, TileType.door);
            setTile(grid.roomID, grid.roomX + 1, grid.roomY, _rooms, 22);
            setTile(grid.roomID, grid.roomX - 1, grid.roomY, _rooms, 24);

            setTile(sGrid.roomID, sGrid.roomX, sGrid.roomY, _rooms);
            setTile(sGrid.roomID, sGrid.roomX, sGrid.roomY - 1, _rooms);
            setTile(sGrid.roomID, sGrid.roomX + 1, sGrid.roomY, _rooms, 14);
            setTile(sGrid.roomID, sGrid.roomX - 1, sGrid.roomY, _rooms, 16);

            setTile(sGrid.roomID, sGrid.roomX + 1, sGrid.roomY - 1, _rooms, 22);
            setTile(sGrid.roomID, sGrid.roomX - 1, sGrid.roomY - 1, _rooms, 24);
        }
        if (_dir.x == 1)
        {
            var eGrid = _grid.grids[tx + 1, ty];
            setTile(grid.roomID, grid.roomX, grid.roomY, _rooms, 10);
            setTile(grid.roomID, grid.roomX, grid.roomY + 1, _rooms, 0, TileType.door);
            setTile(grid.roomID, grid.roomX, grid.roomY - 1, _rooms, 20);
            setTile(grid.roomID, grid.roomX, grid.roomY + 2, _rooms, 21);

            setTile(eGrid.roomID, eGrid.roomX, eGrid.roomY, _rooms, 10);
            setTile(eGrid.roomID, eGrid.roomX, eGrid.roomY - 1, _rooms, 20);
            setTile(eGrid.roomID, eGrid.roomX, eGrid.roomY + 2, _rooms, 21);
            setTile(eGrid.roomID, eGrid.roomX, eGrid.roomY + 1, _rooms);
        }
        if (_dir.x == -1)
        {
            var wGrid = _grid.grids[tx - 1, ty];
            setTile(grid.roomID, grid.roomX, grid.roomY, _rooms, 10);
            setTile(grid.roomID, grid.roomX, grid.roomY + 1, _rooms, 0, TileType.door);
            setTile(grid.roomID, grid.roomX, grid.roomY - 1, _rooms, 20);
            setTile(grid.roomID, grid.roomX, grid.roomY + 2, _rooms, 21);

            setTile(wGrid.roomID, wGrid.roomX, wGrid.roomY, _rooms, 10);
            setTile(wGrid.roomID, wGrid.roomX, wGrid.roomY - 1, _rooms, 20);
            setTile(wGrid.roomID, wGrid.roomX, wGrid.roomY + 2, _rooms, 21);
            setTile(wGrid.roomID, wGrid.roomX, wGrid.roomY + 1, _rooms);
        }
    }

    void setTile(int id, int x, int y, List<SingleRoom> rooms, int sp = 0,TileType type=TileType.floor)
    {
        var curRoom = rooms[id-1];
        if (sp == 0)
        {
            sp = Random.Range(0, 7);
            curRoom.grid[x, y] = TileType.floor;
            if (type != TileType.floor)
                curRoom.grid[x, y] = type;
        }
        curRoom.tiles[x, y].GetComponent<SpriteRenderer>().sprite = _pools.input.spriteList.sprites[_grid.name][sp];
        int i = y * curRoom.width + x;
        //Debug.Log("x:" + x + " ," + "y:" + y + " ,i: " + i+",data:"+curRoom.Data.Length);
        curRoom.data[i] = sp + 1;
    }

    bool checkArea(int x, int y, int w, int h)
    {
        for (int i = x; i < w; i++)
        {
            for (int j = y; j < h; j++)
            {
                if (_grid.grids[i, j].type != TileType.empty)
                    return false;
            }
        }
        return true;
    }

    void selectPoint(ref int roomID)
    {
        bool NotInRoof = true;
        int x, y;
        TileType top, bottom, left, right;
        var grids = _grid.grids;
        do
        {
            x = Random.Range(2, _grid.width - 2);
            y = Random.Range(2, _grid.height - 2);
            if (grids[x, y].type == TileType.roof)
            {
                top = grids[x, y + 1].type;
                bottom = grids[x, y - 1].type;
                left = grids[x - 1, y].type;
                right = grids[x + 1, y].type;
                bool leftRight = left == TileType.roof && right == TileType.roof;
                bool topBottom = top == TileType.roof && bottom == TileType.roof;
                if (top == TileType.empty && leftRight)
                {
                    _dir = new Vector2(0,1);
                    NotInRoof = false;
                }
                else if (bottom == TileType.wall_out && leftRight)
                {
                    _dir = new Vector2(0,-1);
                    NotInRoof = false;
                }
                else if (left == TileType.empty && topBottom)
                {
                    _dir = new Vector2(-1,0);
                    NotInRoof = false;
                }
                else if (right == TileType.empty && topBottom)
                {
                    _dir = new Vector2(1,0);
                    NotInRoof = false;
                }
                roomID = grids[x, y].roomID;
            }
        } while (NotInRoof);
        _doorPos = new Vector2(x, y);
    }

    bool setPoint(out string roomName, ref Vector2 pos, ref int fw, ref int fh)
    {
        Vector2 Pos = _doorPos;
        roomName = randomName();
        RoomExtension.setWH(out fw, out fh, roomName, _pools.input.fileList.fileDic[Res.RoomsXml]);
        if (_dir.y == -1 || _dir.y == 1)
        {
            if (Pos.x - fw / 2 - 1 < 1 || Pos.x + fw / 2 + 1 > Res.columns - 1)
                return false;
            Pos.x -= Mathf.CeilToInt(fw * 0.5f);
            if (_dir.y == 1)
            {
                Pos.y += 1;
                if (Pos.y + fh > Res.rows - 1)
                    return false;
            }
            else
            {
                Pos.y -= fh + 1;
                if (Pos.y < 1)
                    return false;
            }
        }
        else
        {
            if (Pos.y - fh / 2 - 1 < 1 || Pos.y + fh / 2 + 1 > Res.rows - 1)
                return false;
            Pos.y -= Mathf.CeilToInt(fh * 0.5f);
            if (_dir.x == 1)
            {
                Pos.x += 1;
                if (Pos.x + fw > Res.columns - 1)
                    return false;
            }
            else
            {
                Pos.x -= fw;
                if (Pos.x < 1)
                    return false;
            }
        }
        pos = Pos;
        return true;
    }

    void createFirstRoom(string roomName)
    {
        var grid = _grid;
        var pos = new Vector2(grid.width/2, grid.height/2);
        createRoom(roomName, pos);
    }

    void createRoom(string roomName,Vector2 pos)
    {
        int width, height;
        RoomExtension.setWH(out width, out height, roomName, _pools.input.fileList.fileDic[Res.RoomsXml]);
        BuildRoom(roomName, width, height,pos);
        _roomCount++;
    }

    string randomName()
    {
        int length = Enum.GetNames(typeof(Res.Rooms)).Length;
        return Enum.GetName(typeof(Res.Rooms), UnityEngine.Random.Range(0, length));
    }

    void BuildRoom(string name, int width, int height,Vector2 pos)
    {
        GameObject go = new GameObject(_roomCount + "_" + name);
        go.transform.SetParent(_trans, false);
        TileMap _map = TmxLoader.Parse(Res.RoomsPath + name);
        var room = setRoom(name, width, height, pos);
        foreach (var layer in _map.Layers)
        {
            BuildLayer(layer, room, go);
        }
        _grid.rooms.Add(room);
    }

    SingleRoom setRoom(string name, int width, int height,Vector2 pos)
    {
        var room = new SingleRoom();
        room.data = new int[width * height];
        room.grid = new TileType[width, height];
        room.tiles = new GameObject[width, height];
        room.pos = pos;
        room.id = _roomCount;
        room.width = width;
        room.height = height;
        room.name = name;
        room.dir = _dir;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                room.grid[x, y] = TileType.empty;
            }
        }
        return room;
    }

    void BuildLayer(TiledLayer layer, SingleRoom room, GameObject parent)
    {
        int i = 0;
        for (int row = 0; row < layer.Height; row++)
        {
            for (int col = 0; col < layer.Width; col++)
            {
                int gid = layer.Data[col + row * layer.Width];
                if (gid != 0)
                {
                    if (gid < 10)
                        if (Random.Range(0, 15) > 1)
                            gid = Random.Range(1, 7);
                        else
                            gid = Random.Range(7, 11);

                    room.data[i] = gid;
                    GameObject tile = AddTile(layer, row, col, gid, room, layer.Height);
                    var type = (TileType)Enum.Parse(typeof(TileType), layer.Name);
                    room.grid[col, row] = type;
                    if (tile != null)
                    {
                        room.tiles[col, row] = tile;
                        tile.transform.SetParent(parent.transform);
                    }
                }
                i++;
            }
        }
    }

    GameObject AddTile(TiledLayer layer, int y, int x, int gid, SingleRoom room, int height)
    {
        GameObject go = new GameObject(string.Format("{0},{1},{2}",room.id, x, y), typeof(SpriteRenderer));
        go.AddComponent<BoxCollider2D>();
        var type = (TileType)Enum.Parse(typeof(TileType), layer.Name);
        int xx = x;
        int yy = -y + height;

        //建立位置
        var pos = new Vector2(xx, yy) + room.pos;
        go.transform.position = pos;
        //在北边的情况
        if (_dir.y == 1)
        {
            if (_grid.grids[(int)room.pos.x + xx, (int)room.pos.y + yy].type != TileType.empty)
            {
                GameObject.Destroy(go);
                room.data[y * room.width + x] = 0;
                return null;
            }
        }
        //在南边的情况
        if (_dir.y == -1)
        {
            var grid = _grid.grids[(int)room.pos.x + xx, (int)room.pos.y + yy];
            if (grid.type != TileType.empty)
            {
                GameObject.Destroy(go);
                var curRoom = _grid.rooms[grid.roomID-1];
                var obj = curRoom.tiles[grid.roomX, grid.roomY];
                curRoom.data[grid.roomY * curRoom.width + grid.roomX] = 0;
                obj.GetComponent<SpriteRenderer>().sprite = _pools.input.spriteList.sprites[_grid.name][gid - 1];
                grid.roomID = _roomCount;
                grid.type = type;
                grid.roomX = xx;
                grid.roomY = yy;
                obj.name = string.Format("{0},{1},{2}", _roomCount, xx, yy);
                return obj;
            }
        }

        //建立画布属性
        _grid.grids[(int)pos.x, (int)pos.y].type = type;
        _grid.grids[(int)pos.x, (int)pos.y].roomID = room.id;
        _grid.grids[(int)pos.x, (int)pos.y].roomX = x;
        _grid.grids[(int)pos.x, (int)pos.y].roomY = y;

        //建立精灵
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.sprite = _pools.input.spriteList.sprites[_grid.name][gid - 1];
        return go;
    }
}

