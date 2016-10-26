using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class AddViewSystem : ISetPools, IInitializeSystem, IEntityCollectorSystem
{
    public EntityCollector entityCollector { get { return _entityCollector; } }
    Pools _pools;
    EntityCollector _entityCollector;

    public void SetPools(Pools pools)
    {
        _pools = pools;
        _entityCollector = new[] { pools.board, pools.core }
            .CreateEntityCollector(Matcher.AnyOf(BoardMatcher.Asset,CoreMatcher.Asset));
    }

    public void Initialize()
    {
        foreach (var pool in _pools.allPools)
        {
            Transform container = new GameObject(pool.metaData.poolName + " Views").transform;
            _pools.input.holder.poolDic.Add(StringExtension.GetInPools(pool.metaData.poolName), container);
        }

    }

    public void Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            var gameObject = AssetsExtension.Instantiate<GameObject>(Res.PrefabPath + e.asset.name);
            gameObject.transform.SetParent(_pools.input.holder.poolDic[e.pool.name], false);
            gameObject.Link(e, PoolExtension.GetPool(e.pool.name));
            e.AddView(gameObject.GetComponent<IViewController>());
        }
    }

}
