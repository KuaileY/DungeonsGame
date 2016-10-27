using System.Collections.Generic;
using Entitas;
using UniRx;
using UnityEngine;

public sealed class TurnSystem : IReactiveSystem, ISetPools
{
    public TriggerOnEvent trigger { get { return InputMatcher.ActiveTurn.OnEntityAdded(); } }
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }
    public void Execute(List<Entity> entities)
    {
        Vector2 dir;
        dirGet(out dir);
        Vector2 Pos = _pools.core.controlableEntity.view.controller.position;
        //玩家行为
        RoleBehaviour(Pos + dir, dir);
        _pools.input.DestroyEntity(entities.SingleEntity());
    }

    void RoleBehaviour(Vector2 pos, Vector2 dir)
    {
        move(pos);
    }

    void move(Vector2 pos)
    {
        _pools.core.controlableEntity.ReplacePosition(0, pos);
        _pools.core.controlableEntity.AddMove(0);
    }
    void dirGet(out Vector2 dir)
    {
        var input = _pools.input.input;
        Vector2 tmp=new Vector2(0,0);
        _pools.core.GetEntities()
            .ToObservable()
            .Where(x => x.hasAsset)
            .Where(x => x.asset.name == Res.player)
            .Subscribe(xx =>
            {
                tmp = InputExtension.getDir(new Vector3(input.x, input.y, 0) - xx.position.value);
            });
        dir = tmp;
    }

}