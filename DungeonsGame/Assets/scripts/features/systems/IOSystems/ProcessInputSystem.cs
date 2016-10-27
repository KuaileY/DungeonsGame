using System.Collections.Generic;
using Entitas;
using UniRx;
using UnityEngine;

public sealed class ProcessInputSystem :ISetPools,IReactiveSystem
{
    public TriggerOnEvent trigger { get { return InputMatcher.Input.OnEntityAdded(); } }
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }

    public void Execute(List<Entity> entities)
    {
        var inputEntity = entities.SingleEntity();
        var input = inputEntity.input;


        //激活当前回合（触发TrunSystem)
        _pools.input.isActiveTurn = true;
    }

}

