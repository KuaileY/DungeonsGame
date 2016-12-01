using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UniRx;
using Entitas;
using Object = UnityEngine.Object;

public class CreateInteractiveObjSystem : IReactiveSystem, ISetPools
{
    public TriggerOnEvent trigger
    {
        get { return CoreMatcher.Dungeon.OnEntityAdded(); }
    }

    Pools _pools;

    public void SetPools(Pools pools)
    {
        _pools = pools;
    }

    public void Execute(List<Entity> entities)
    {
        var data = _pools.input.fileList.fileDic[Res.cache.Interactive.ToString()];
        foreach (var entity in entities )
        {
            if (entity.dungeon.value == 0)
                createPlayer(data);
            else
            {
                createDoor(data);
            }
        }
        //清除实体
        _pools.core.DestroyEntity(_pools.core.dungeonEntity);
    }



    #region create

    void createDoor(XDocument data)
    {
        data.Elements().First().Elements().ToObservable()
            .Where(x => x.Element("door") != null)
            .Do(x =>
            {
                var roomId = x.Attribute("id").Value.toInt();
                var xElement = x.Element("door").Value.cleanEnd();
                var posArray = xElement.Split(',');
                int xx = x.Attribute("x").Value.toInt();
                int yy = x.Attribute("y").Value.toInt();
                foreach (var pos in posArray)
                {
                    var p = pos.Split('|');
                    _pools.core.CreateEntity()
                        .AddPosition(roomId, new Vector3(p[0].toInt() + xx, p[1].toInt() + yy))
                        .AddPool(Res.InPools.Core)
                        .AddRoom(roomId, roomId)
                        .IsInteractive(true)
                        .AddViewObject(ObjectsIndeies.I_L_door);
                }
            })
            .Subscribe();
    }

    void createPlayer(XDocument data)
    {
        data.Elements().First().Elements().ToObservable()
            .Where(x => x.Element("objectGroup") != null)
            .Do(x =>
            {
                int roomx = x.Attribute("x").Value.toInt();
                int roomy = x.Attribute("y").Value.toInt();
                int roomH = x.Attribute("height").Value.toInt();
                foreach (var e in x.Element("objectGroup").Elements())
                {
                    if (e.Attribute("name").Value == "born")
                    {
                        int xx = roomx + e.Attribute("x").Value.toInt();
                        int yy = roomy + roomH - e.Attribute("y").Value.toInt();
                        _pools.core.CreateEntity()
                            .AddPosition(0, new Vector3(xx, yy))
                            .AddPool(Res.InPools.Core)
                            .AddAsset("Player")
                            .AddRoom(1, 1)
                            .IsControlable(true)
                            .IsInteractive(true);
                    }
                }
                
            })
            .Subscribe();


    }

    #endregion
}