using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using Entitas;
using Object = UnityEngine.Object;

public class CreateInteractiveObjSystem : IReactiveSystem, ISetPools
{
    public TriggerOnEvent trigger { get { return CoreMatcher.Dungeon.OnEntityAdded(); } }
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }
    public void Execute(List<Entity> entities)
    {

        //清除实体
        _pools.core.DestroyEntity(_pools.core.dungeonEntity);
    }

}

