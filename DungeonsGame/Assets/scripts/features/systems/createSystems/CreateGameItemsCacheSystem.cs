using System.Collections.Generic;
using Entitas;
public sealed class CreateGameItemsCacheSystem:ISystem,ISetPools
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
//         var gameItem = _pool.GetGroup(Matcher.AllOf(CoreMatcher.Interactive, CoreMatcher.Position));
//         gameItem.OnEntityAdded += (group, entity, index, component) =>
//         {
//             TestLoadConfig.log.Trace("gameItem.OnEntityAdded");
//             var grid = _pool.gameBoardCache.grid;
//             grid[entity.position.x, entity.position.y] = entity;
//             _pool.ReplaceGameBoardCache(grid);
//         };
// 
//         gameItem.OnEntityRemoved += (group, entity, index, component) =>
//         {
//             TestLoadConfig.log.Trace("gameItem.OnEntityRemoved");
//             var pos = component as PositionComponent;
//             if (pos == null)
//                 pos = entity.position;
//             var grid = _pool.gameBoardCache.grid;
//             grid[pos.x, pos.y] = null;
//             pools.core.ReplaceGameBoardCache(grid);
//         };
    }

    void CreateGameItemsCache(ItemBoardComponent gameItems)
    {
        int i = 0;
        foreach (var room in gameItems.roomList)
        {
            var grid = new Entity[room.columns, room.rows];
            foreach (var entity in _pool.GetEntities(CoreMatcher.Interactive))
            {
                grid[(int)entity.position.value.x, (int)entity.position.value.y] = entity;
            }
            if (entityList[i] != null)
                entityList[i] = grid;
            else
                entityList.Add(grid);
            i++;
        }
        _pool.ReplaceGameItemsCache(entityList);
    }

}

