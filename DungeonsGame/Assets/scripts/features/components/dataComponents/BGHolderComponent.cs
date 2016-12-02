using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

[Input,SingleEntity]
public sealed class BGHolderComponent:IComponent
{
    public GameObject[,] goArray;
}

