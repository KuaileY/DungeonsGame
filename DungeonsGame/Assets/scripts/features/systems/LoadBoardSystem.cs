using System;
using System.Collections.Generic;
using System.Xml;
using Entitas;
using UnityEngine;

public sealed class LoadBoardSystem:IReactiveSystem, ISetPools
{
    public TriggerOnEvent trigger { get { return BoardMatcher.LoadBoard.OnEntityAdded(); } }
    Pools _pools;
    //保持对象
    Transform _trans;
    //当前画布
    SingleGrid _grid;
    //精灵图片
    Sprite[] _sprites;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }
    public void Execute(List<Entity> entities)
    {

        _trans = _pools.input.holder.poolDic[entities[0].pool.name];
        int floor = _pools.board.loadBoard.floor;
        createBoard(floor);
    }

    void createBoard(int floor)
    {
        var levelData = Res.Files.levelData.ToString();
        XmlDocument xdoc = _pools.input.fileList.fileDic[levelData];
        if (xdoc != null)
        {
            XmlNodeList grids=xdoc.SelectNodes("level/grid");
            foreach (var grid in grids)
            {
                var curGrid = BoardExtension.loadGrid((XmlElement)grid);
                LevelData.grids.Add(curGrid);
            }
            _grid = LevelData.grids[floor-1];
            _sprites = Resources.LoadAll<Sprite>(Res.mapsTexturePath + _grid.name);
            foreach (var room in _grid.rooms)
            {
                TileMap map = TmxLoader.Parse(Res.RoomsPath + room.name);
                loadRoom(room, map);
            }
        }
    }
    void loadRoom(SingleRoom room, TileMap map)
    {
        GameObject go = new GameObject(room.name);
        go.transform.SetParent(_trans);
        int i = 0;
        for (var row = 0; row < room.height; row++)
        {
            for (var col = 0; col < room.width; col++)
            {
                int gid = room.data[col + row * room.width];
                if (gid != 0)
                {
                    GameObject tile = LoadTile(row, col, gid, room, room.height);
                    if (tile != null)
                    {
                        room.tiles[col, row] = tile;
                        tile.transform.SetParent(go.transform);
                        room.data[i] = gid;
                    }
                }
                i++;
            }
        }
    }

    GameObject LoadTile(int y, int x, int gid, SingleRoom room, int height)
    {
        GameObject go = new GameObject(string.Format("{0},{1}", x, y), typeof(SpriteRenderer));
        int xx = x;
        int yy = -y + height;
        go.transform.position = new Vector2(xx, yy) + room.pos;
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        sr.sprite = _sprites[gid - 1];
        return go;
    }


}

