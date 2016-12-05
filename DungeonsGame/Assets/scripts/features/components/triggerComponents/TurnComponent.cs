using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

[Input,SingleEntity]
public sealed class TurnComponent:IComponent
{
    public string type;
    public Vector2 dir;
}

