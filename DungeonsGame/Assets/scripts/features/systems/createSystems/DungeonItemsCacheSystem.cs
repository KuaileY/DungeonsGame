using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class DungeonItemsCacheSystem:ISystem,ISetPools
{
    Pool _pool;
    List<Entity[,]> entityList = new List<Entity[,]>();
    public void SetPools(Pools pools)
    {
//         _pool = pools.core;
// 
//         //初始化GameItemsCache
//         var gameItems = pools.core.GetGroup(Matcher.AnyOf( CoreMatcher.ItemBoard));
//         gameItems.OnEntityAdded += (group, entity, index, component) =>
//         {
//             CreateGameItemsCache((ItemBoardComponent)component);
//         };
// 
//         
//         //添加Items
//         var gameItem = _pool.GetGroup(Matcher.AllOf(CoreMatcher.Interactive, CoreMatcher.Position));
//         gameItem.OnEntityAdded += (group, entity, index, component) =>
//         {
//             //TestLoadConfig.log.Trace("dungeonItem.OnEntityAdded");
//             var grid = _pool.dungeonItemsCache.roomList[entity.room.roomId];
//             var room=_pool.itemBoard.roomList.rooms[entity.room.roomId];
//             var x = (int)(entity.position.value.x - room.pos.x);
//             var y = (int) (entity.position.value.y - room.pos.y);
//             //Debug.Log("roomId:" + entity.room.roomId + " ,x:" + x + " ,y:" + (y - 2));
//             if (entity.room.roomId != entity.room.oldRoomId)
//             {
//                 if (entity.dir.value.x == 1)
//                     grid[x , y - 2] = entity;
//                 if (entity.dir.value.x == -1)
//                     grid[x - 1, y - 2] = entity;
//                 if (entity.dir.value.y == 1)
//                     grid[x, y - 2] = entity;
//                 if (entity.dir.value.y == -1)
//                     grid[x, y - 2] = entity;
//             }
//             else
//                 grid[x, y - 2] = entity;
//             entityList[entity.room.roomId] = grid;
//             _pool.ReplaceDungeonItemsCache(entityList);
//         };
// 
//         gameItem.OnEntityRemoved += (group, entity, index, component) =>
//         {
//             //TestLoadConfig.log.Trace("gameItem.OnEntityRemoved");
//             var pos = component as PositionComponent;
//             if (pos == null)
//                 pos = entity.position;
//             int roomId;
//             var grid = _pool.dungeonItemsCache.roomList[entity.room.oldRoomId];
//             var roomPos = _pool.itemBoard.roomList.rooms[entity.room.oldRoomId].pos;
//             var x = (int)(pos.value.x-roomPos.x);
//             var y = (int)(pos.value.y-roomPos.y);
//             grid[x, y-2] = null;
//             entityList[entity.room.oldRoomId] = grid;
//             _pool.ReplaceDungeonItemsCache(entityList);
//         };
//         
    }

//     void CreateGameItemsCache(ItemBoardComponent gameItems)
//     {
//         entityList.Clear();
//         foreach (var room in gameItems.roomList.rooms)
//         {
//             var grid = new Entity[room.width, room.height-1];
//             foreach (var entity in _pool.GetEntities(CoreMatcher.Interactive))
//             {
//                 grid[(int)entity.position.value.x, (int)entity.position.value.y] = entity;
//             }
//             entityList.Add(grid);
//         }
//         _pool.ReplaceDungeonItemsCache(entityList);
//     }

}

