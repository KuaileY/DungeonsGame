using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class MainLightSystem:IReactiveSystem,ISetPools
{
    public TriggerOnEvent trigger { get { return CoreMatcher.Controlable.OnEntityAdded(); } }
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }
    public void Execute(List<Entity> entities)
    {
        var go = new GameObject("MainLight");
        var light = go.AddComponent<Light>();
        go.transform.position = entities.SingleEntity().position.value;
        go.transform.position += new Vector3(0, 0, -1.5f);
        light.type = LightType.Point;
        light.range = 4;
        light.color = new Color(1.0f, 0.8f, 0.5f, 1.0f);
        light.intensity = 3.2f;
    }

}

