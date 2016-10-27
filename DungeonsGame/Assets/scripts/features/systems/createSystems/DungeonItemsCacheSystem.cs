using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class DungeonItemsCacheSystem:ISystem,ISetPools
{
    Pool _pool;
    List<Entity[,]> entityList = new List<Entity[,]>();
    public void SetPools(Pools pools)
    {
        _pool = pools.core;

        //初始化GameItemsCache
        var gameItems = pools.core.GetGroup(Matcher.AnyOf( CoreMatcher.ItemBoard));
        gameItems.OnEntityAdded += (group, entity, index, component) =>
        {
            CreateGameItemsCache((ItemBoardComponent)component);
        };
//         gameBoard.OnEntityUpdated += (group, entity, index, previousComponent, newComponent) =>
//         {
//             CreateGameItemsCache((GameBoardComponent)newComponent);
//         };

        //添加Items
        var gameItem = _pool.GetGroup(Matcher.AllOf(CoreMatcher.Interactive, CoreMatcher.Position));
        gameItem.OnEntityAdded += (group, entity, index, component) =>
        {
            //TestLoadConfig.log.Trace("dungeonItem.OnEntityAdded");
            var grid = _pool.dungeonItemsCache.roomList[entity.position.roomId];
            var pos=_pool.itemBoard.roomList.rooms[entity.position.roomId].pos;
            var x = (int)(entity.position.value.x - pos.x);
            var y = (int) (entity.position.value.y - pos.y);
            grid[x, y-1] = entity;
            entityList[entity.position.roomId] = grid;
            _pool.ReplaceDungeonItemsCache(entityList);
        };

        gameItem.OnEntityRemoved += (group, entity, index, component) =>
        {
            //TestLoadConfig.log.Trace("gameItem.OnEntityRemoved");
            var pos = component as PositionComponent;
            if (pos == null)
                pos = entity.position;
            var grid = _pool.dungeonItemsCache.roomList[entity.position.roomId];
            var roomPos = _pool.itemBoard.roomList.rooms[entity.position.roomId].pos;
            var x = (int)(pos.value.x-roomPos.x);
            var y = (int)(pos.value.y-roomPos.y);
            grid[x, y-1] = null;
            entityList[entity.position.roomId] = grid;
            _pool.ReplaceDungeonItemsCache(entityList);
        };

    }

    void CreateGameItemsCache(ItemBoardComponent gameItems)
    {
        entityList.Clear();
        foreach (var room in gameItems.roomList.rooms)
        {
            var grid = new Entity[room.width, room.height];
            foreach (var entity in _pool.GetEntities(CoreMatcher.Interactive))
            {
                grid[(int)entity.position.value.x, (int)entity.position.value.y] = entity;
            }
            entityList.Add(grid);
        }
        _pool.ReplaceDungeonItemsCache(entityList);
    }

}

