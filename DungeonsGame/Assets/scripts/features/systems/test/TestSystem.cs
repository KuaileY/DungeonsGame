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
    public void SetPools(Pools pools)
    {
        _entityCollector = new[] {pools.board, pools.core,pools.input}
            .CreateEntityCollector(Matcher.AnyOf(InputMatcher.Test,BoardMatcher.Test,CoreMatcher.Test));
    }
    public void Initialize()
    {

        
    }


    public void Execute(List<Entity> entities)
    {
        //lala
    }


}

