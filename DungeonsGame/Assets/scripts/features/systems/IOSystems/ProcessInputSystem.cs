using System;
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
        if (_pools.input.isProcessing)
            return;

        foreach (var entity in entities )
        {
            var tapType = entity.input.name[0];
            ProcessType(tapType, entity);
        }
    }

    private void ProcessType(char tapType, Entity entity)
    {
        _pools.input.isProcessing = true;
        var pos = new Vector2(entity.input.x, entity.input.y);
        if (tapType.ToString().IsInt())
        {
            Debug.Log("path finding");
            _pools.input.CreateEntity().AddActiveTurn(pos, playerBehavior.astar.ToString());
        }
        else
            ProcessItem(tapType, pos);
    }

    private void ProcessItem(char tapType, Vector2 pos)
    {
        switch (tapType)
        {
            case 'I':
                Debug.Log("item");
                _pools.input.CreateEntity().AddActiveTurn(pos, playerBehavior.pick.ToString());
                break;
            case 'M':
                Debug.Log("monster");
                _pools.input.CreateEntity().AddActiveTurn(pos, playerBehavior.attack.ToString());
                break;
            case 'P':
                Debug.Log("player");
                _pools.input.isProcessing = false;
                break;
            case 'O':
                Debug.Log("obstacle");
                _pools.input.CreateEntity().AddActiveTurn(pos, playerBehavior.astar.ToString());
                break;
            case 'N':
                Debug.Log("null");
                _pools.input.isProcessing = false;
                break;
            case 'B':
                Debug.Log("building");
                _pools.input.CreateEntity().AddActiveTurn(pos, playerBehavior.astar.ToString());
                break;
            default:
                throw new Exception("game name is wrong!");
        }
    }
}

