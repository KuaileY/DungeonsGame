using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Entitas;
using OfficeOpenXml;
using UnityEngine;

public sealed class TestSystem : IInitializeSystem, IEntityCollectorSystem,ISetPools
{
    public EntityCollector entityCollector { get { return _entityCollector; } }
    EntityCollector _entityCollector;
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
        _entityCollector = new[] {pools.board, pools.core,pools.input}
            .CreateEntityCollector(Matcher.AnyOf(InputMatcher.Test,BoardMatcher.Test,CoreMatcher.Test));
    }
    public void Initialize()
    {
        // 创建保持对象
        //_pools.input.CreateEntity().AddHolder(new Dictionary<Res.InPools, Transform>());
    }


    public void Execute(List<Entity> entities)
    {
        //lala
        var ss = "Items/" + ObjectsIndeies.I_L_door;
        _pools.core.CreateEntity()
            .AddPosition(new Vector2(0, 0))
            .AddPool(Res.InPools.Core)
            .IsInteractive(true)
            .AddAsset(ss);

        Debug.Log("test");
        foreach (var entity in entities )
        {
            entity.IsDestroy(true);
        }
    }


}

