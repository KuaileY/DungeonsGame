using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

[Input, SingleEntity]
public sealed class ActiveTurnComponent : IComponent
{
    public Vector2 pos;
    public string type;
}