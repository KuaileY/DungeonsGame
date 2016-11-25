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
        CreateInteractiveObjExtension.CreateObj(_pools, "door", 1, 1);
        //清除实体
        _pools.core.DestroyEntity(_pools.core.dungeonEntity);
    }

    private void CreateDoor()
    {
        var data = _pools.input.fileList.fileDic[Res.cache.Interactive.ToString()].Elements().First();
        GameObject go = Resources.Load<GameObject>(Res.PrefabPath + Res.Prefabs.food);

        data.Elements().ToObservable()
            .Where(x => x.Element("door") != null)
            .Do(x =>
            {
                var value = x.Element("door").Value.TrimEnd(',');
                var PosArrays = value.Split(',');
                foreach (var posArray in PosArrays)
                {
                    var pos = posArray.Split('|');
                    Object.Instantiate(go, new Vector2((float.Parse(pos[0])), float.Parse(pos[1])), Quaternion.identity);
                }
            })
            .Subscribe();

    }

}

