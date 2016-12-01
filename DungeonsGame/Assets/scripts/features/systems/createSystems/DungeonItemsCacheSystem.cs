using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Entitas;
using UniRx;
using UnityEngine;

public sealed class DungeonItemsCacheSystem:ISystem,ISetPools
{
    List<Entity[,]> entityList = new List<Entity[,]>();
    public void SetPools(Pools pools)
    {
        //初始化GameItemsCache
        var gameItems = pools.board.GetGroup(Matcher.AnyOf( BoardMatcher.GameBoard));
        gameItems.OnEntityRemoved += (group, entity, index, component) =>
        {
            CreateGameItemsCache(pools);
        };

        
        //添加Items
        var gameItem = pools.core.GetGroup(Matcher.AllOf(CoreMatcher.Interactive, CoreMatcher.Position));
        gameItem.OnEntityAdded += (group, entity, index, component) =>
        {
            //TestLoadConfig.log.Trace("dungeonItem.OnEntityAdded");
            var data = pools.input.fileList.fileDic[Res.cache.Interactive.ToString()];
            var room = data.Elements().First().Element("room" + entity.room.roomId);
            int xx = (int) entity.position.value.x - room.Attribute("x").Value.toInt();
            int yy = (int) entity.position.value.y - room.Attribute("y").Value.toInt();

            var grid = pools.board.dungeonItemsCache.roomList[entity.room.roomId - 1];
            grid[xx, yy - 2] = entity;
            entityList[entity.room.roomId - 1] = grid;
            pools.board.ReplaceDungeonItemsCache(entityList);
        };

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

    void CreateGameItemsCache(Pools pools)
    {
        entityList.Clear();
        var data = pools.input.fileList.fileDic[Res.cache.Interactive.ToString()];
        data.Elements().First().Elements()
            .OrderBy(x=>x.Attribute("id").Value.toInt())
            .ToObservable()
            .Do(x =>
            {
                var grid = new Entity[x.Attribute("width").Value.toInt(), x.Attribute("height").Value.toInt()-1];
                var posArray=x.Element("obstacleData").Value.cleanEnd().Split(',');
                int xx = x.Attribute("x").Value.toInt();
                int yy = x.Attribute("y").Value.toInt();
                int id = x.Attribute("id").Value.toInt();
                foreach (var pos in posArray)
                {
                    var p = pos.Split('|');
                    if (p[1].toInt() > 1 )
                    {
                        var entity = pools.core.CreateEntity()
                            .AddPosition(id, new Vector3(p[0].toInt() + xx, p[1].toInt() + yy))
                            .AddPool(Res.InPools.Core)
                            .AddViewObject(ObjectsIndeies.I_P_shadow);

                        grid[p[0].toInt(), p[1].toInt()-2] = entity;
                    }
                }
                entityList.Add(grid);
            })
            .Subscribe();
        pools.board.ReplaceDungeonItemsCache(entityList);
    }

}

