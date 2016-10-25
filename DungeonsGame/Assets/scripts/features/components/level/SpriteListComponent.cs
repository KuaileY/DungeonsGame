using System.Collections.Generic;
using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

[Input,SingleEntity]
public sealed class SpriteListComponent:IComponent
{
    public Dictionary<string, Sprite[]> sprites;
}

