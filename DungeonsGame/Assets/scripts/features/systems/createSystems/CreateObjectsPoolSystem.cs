using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class CreateObjectsPoolSystem : IReactiveSystem, ISetPools
{
    public TriggerOnEvent trigger { get { return InputMatcher.ViewObjectPool.OnEntityAdded(); } }
    Pools _pools;
    MyObjectPool<GameObject> _objectPool;

    public void SetPools(Pools pools)
    {
        _pools = pools;
        var indeies = ObjectsIndeies.indexNames;
        _objectPool = new MyObjectPool<GameObject>();
        Transform parent = new GameObject("ObjectsPool").transform;
        for (int i = 0; i < indeies.Length ; i++)
        {
            string path=String.Empty;
            if (indeies[i][0] == 'I')
                path = Res.PrefabPath + "Items/" + indeies[i];
            if (indeies[i][0] == 'M')
                path = Res.PrefabPath + "Monster/" + indeies[i];

            if (path == String.Empty)
                throw new Exception("objectPoolName is wrong!");
            indeies[i].print();
            var go = Assets.Instantiate<GameObject>(path);
            go.transform.SetParent(parent);
            _objectPool.Pop(go, indeies[i]);
        }
    }

    public void Execute(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            entity.IsDestroy(true);
        }
    }

}

