using Entitas;
using UnityEngine;

[Input]
public sealed class ViewObjectPoolComponent:IComponent
{
    public MyObjectPool<GameObject> pool;
}

