using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

[Input,SingleEntity]
public sealed class ViewObjectPoolComponent:IComponent
{
    public MyObjectPool<GameObject> pool;
}

