using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class AddViewFromObjectPoolSystem:IReactiveSystem,ISetPools
{
    public TriggerOnEvent trigger { get { return CoreMatcher.ViewObject.OnEntityAdded(); } }
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }
    public void Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            var originGo = _pools.input.viewObjectPool.pool.Get(e.viewObject.name);
            var go = Object.Instantiate<GameObject>(originGo);
            go.SetActive(true);
            go.transform.SetParent(_pools.input.holder.poolDic[e.pool.name], false);
            go.Link(e, _pools.core);
            e.AddView(go.GetComponent<IViewController>());
        }
    }

}

