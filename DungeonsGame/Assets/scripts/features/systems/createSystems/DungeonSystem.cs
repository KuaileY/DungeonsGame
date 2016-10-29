using System.Collections.Generic;
using Entitas;
using UnityEngine;
using UniRx;

public sealed class DungeonSystem: IReactiveSystem, ISetPools
{
    public TriggerOnEvent trigger { get { return CoreMatcher.Dungeon.OnEntityAdded(); } }
    Group _items;
    Pools _pools;

    //坐标集合，避免重复坐标。
    List<List<Vector2>> PositionList = new List<List<Vector2>>();
    //当前画布
    SingleGrid _grid;
    public void SetPools(Pools pools)
    {
        _pools = pools;
        _items = pools.core.GetGroup(Matcher.AllOf(CoreMatcher.Interactive).NoneOf(CoreMatcher.Controlable));
    }

    public void Execute(List<Entity> entities)
    {
        //Debug.Log("rooms:"+LevelData.grids[0].rooms.Count);
        var entity = entities.SingleEntity();
        _grid = LevelData.grids[entity.dungeon.value - 1];
        //创建条目缓存
        _pools.core.CreateEntity().AddItemBoard(LevelData.grids[0]);
        //清空缓存
        //clearGrid();
        //创建场景
        CreateSence(entity.dungeon.value);
    }

    void CreateSence(int value)
    {
        //做一个坐标集合，以防止元素重叠。
        initialPosList(value);
        //创建玩家角色
        //Debug.Log("x: " + PositionList[0][10].x + " , y: " + PositionList[0][10].y);
        createPlayer(10);
    }


    void initialPosList(int value)
    {
        PositionList.Clear();
        var rooms= LevelData.grids[value-1].rooms;
        
        for (int i = 0; i < rooms.Count; i++)
        {
            var pos = new List<Vector2>();
            for (int y = 0; y < rooms[i].height; y++)
            {
                for (int x = 0; x < rooms[i].width; x++)
                {
                    if (rooms[i].grid[x, y] == TileType.floor)
                    {
                        pos.Add(new Vector2(x, -y + rooms[i].height));
                    }
                    else
                    {
                        if (y != rooms[i].height-1)
                            createObstacle(rooms[i].id - 1, x, -y + rooms[i].height, rooms[i].dir);
                        if(rooms[i].grid[x, y] == TileType.door)
                            createDoor(rooms[i].id - 1, x, -y + rooms[i].height, rooms[i].dir);
                    }
                }
            }
            PositionList.Add(pos);
        }
    }

    private void createDoor(int id, int x, int y, Vector2 dir)
    {
        _pools.core.CreateEntity()
            .AddRoom(id, id)
            .AddPosition(new Vector2(x, y) + _grid.rooms[id].pos)
            .IsInteractive(true)
            .AddDir(dir)
            .AddPool(Res.InPools.Core)
            .AddAsset(Res.door);
    }

    private void createObstacle(int id, int x, int y,Vector2 dir)
    {
        _pools.core.CreateEntity()
            .AddRoom(id, id)
            .AddPosition(new Vector2(x, y) + _grid.rooms[id].pos)
            .IsInteractive(true)
            .AddDir(dir)
            .AddPool(Res.InPools.Core)
            .AddAsset(Res.food);
    }

    void  createPlayer(int i)
    {
        var player = _pools.core.CreateEntity()
            .AddPosition(PositionList[0][i] + _grid.rooms[0].pos)
            .AddRoom(0, 0)
            .IsInteractive(true)
            .IsControlable(true)
            .AddPool(Res.InPools.Core)
            .AddAsset(Res.player);
        var pos = player.position.value + new Vector3(0, 0, -10);
        //建立摄像机位置
        _pools.core.cameraEntity.AddPosition(pos);
    }

}

