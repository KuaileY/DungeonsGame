using System.Collections.Generic;
using Entitas;
using UnityEngine;

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
        createPlayer(0);

    }

    void createPlayer(int i)
    {
        _pools.core.CreateEntity()
            .AddPosition(PositionList[0][i] + _grid.rooms[0].pos )
            .IsInteractive(true)
            .IsControlable(true)
            .AddPool(Res.InPools.Core)
            .AddAsset(Res.player);
    }

    void initialPosList(int value)
    {
        PositionList.Clear();
        var rooms= LevelData.grids[0].rooms;
        var pos = new List<Vector2>();
        for (int i = 0; i < rooms.Count; i++)
        {
            pos.Clear();
            for (int y = 0; y < rooms[i].height; y++)
            {
                for (int x = 0; x < rooms[i].width; x++)
                {
                    if (rooms[i].grid[x, y] == TileType.floor)
                    {
                        pos.Add(new Vector2(x, -y+rooms[i].height));
                    }
                }
            }
            PositionList.Add(pos);
        }
    }

}

