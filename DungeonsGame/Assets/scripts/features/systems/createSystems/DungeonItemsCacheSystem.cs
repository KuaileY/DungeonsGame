using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Entitas;
using UniRx;
using UnityEngine;

public sealed class DungeonItemsCacheSystem:ISystem,ISetPools
{
    Entity[,] grid = new Entity[Res.columns,Res.rows];
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

            int xx = (int) entity.position.value.x;
            int yy = (int) entity.position.value.y;
            grid[xx, yy] = entity;

            pools.board.ReplaceDungeonItemsCache(grid);
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
        var data = pools.input.fileList.fileDic[Res.cache.Interactive.ToString()];
        data.Elements().First().Elements()
            .OrderBy(x=>x.Attribute("id").Value.toInt())
            .ToObservable()
            .Do(x =>
            {
                var posArray=x.Element("obstacleData").Value.cleanEnd().Split(',');
                var waterArray = x.Element("waterData").Value.cleanEnd().Split(',');
                int xx = x.Attribute("x").Value.toInt();
                int yy = x.Attribute("y").Value.toInt();
                int id = x.Attribute("id").Value.toInt();
                foreach (var pos in posArray)
                {
                    var p = pos.Split('|');
                    if (p[1].toInt() > 0 )
                    {
                        var entity = pools.core.CreateEntity()
                            .AddPosition(new Vector3(p[0].toInt() + xx, p[1].toInt() + yy))
                            .AddPool(Res.InPools.Core)
                            .AddViewObject(ObjectsIndeies.I_P_null);

                        grid[p[0].toInt() + xx, p[1].toInt() + yy - 1] = entity;
                    }
                }
                foreach (var pos in waterArray)
                {
                    if (pos != string.Empty)
                    {
                        var p = pos.Split('|');
                        var entity = pools.core.CreateEntity()
                            .AddPosition(new Vector3(p[0].toInt() + xx, p[1].toInt() + yy))
                            .AddPool(Res.InPools.Core)
                            .AddViewObject(ObjectsIndeies.I_D_apple);

                        grid[p[0].toInt() + xx, p[1].toInt() + yy - 1] = entity;
                    }

                }
            })
            .Subscribe();
        pools.board.ReplaceDungeonItemsCache(grid);
    }

}

