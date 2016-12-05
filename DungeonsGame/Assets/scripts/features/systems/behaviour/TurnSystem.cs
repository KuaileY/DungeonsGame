using System;
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
        foreach (var entity in entities)
        {
            if (isNeighbor(entity))
            {
                ExeBehavior(entity,true);
                _pools.input.isProcessing = false;
            }
            else
                ExeBehavior(entity,false);
            _pools.input.DestroyEntity(_pools.input.activeTurnEntity);
        }

    }

    private void ExeBehavior(Entity entity,bool neighbor)
    {
        var type =(playerBehavior)Enum.Parse(typeof(playerBehavior),entity.activeTurn.type);
        switch (type)
        {
            case playerBehavior.attack:
                if (neighbor)
                    Debug.Log("attack");
                else if (isAttackRange())
                    Debug.Log("attack");
                else
                {
                    Debug.Log("describe");
                    _pools.input.isProcessing = false;
                }
                break;
            case playerBehavior.pick:
                if (neighbor)
                    Debug.Log("pick");
                else
                {
                    Debug.Log("describe");
                    _pools.input.isProcessing = false;
                }
                break;
            case playerBehavior.astar:
                pathFinding(entity);
                break;
            default:
                throw new Exception("playerBehavior is wrong!");
        }
    }

    private bool isAttackRange()
    {
        return false;
    }

    bool isNeighbor(Entity entity)
    {
        var player = _pools.core.controlableEntity;
        var dis = Vector2.Distance(entity.activeTurn.pos, player.position.value);
        return dis - 1 < 0.0001;
    }

    void pathFinding(Entity entity)
    {
        //是否有路线
        //是否超过50步
        //是否超过10回合
        //路线是否封堵
        //是否遭到攻击
        //否则循环执行玩家步进
        var player = _pools.core.controlableEntity;
        _pools.board.dungeonItemsCache.grid[(int)player.position.value.x, (int)player.position.value.y] = null;
        AstarExtension.PathFind(player.position.value, entity.activeTurn.pos, _pools, ProcessEnd);


    }

    void ProcessEnd(List<Vector2> posList)
    {
        foreach (var pos in posList )
        {
            pos.print();
        }
        _pools.input.isProcessing = false;
    }

}