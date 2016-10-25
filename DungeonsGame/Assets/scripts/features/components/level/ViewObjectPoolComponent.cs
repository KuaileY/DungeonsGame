using Entitas;
using UnityEngine;

[Board,Core]
public sealed class ViewObjectPoolComponent:IComponent
{
    public ObjectPool<GameObject> pool;
}

