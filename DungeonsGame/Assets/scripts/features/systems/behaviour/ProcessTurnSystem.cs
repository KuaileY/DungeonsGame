using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class ProcessTurnSystem:IReactiveSystem,ISetPools
{
    public TriggerOnEvent trigger { get; private set; }
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }

    public void Execute(List<Entity> entities)
    {
        throw new NotImplementedException();
    }

}

