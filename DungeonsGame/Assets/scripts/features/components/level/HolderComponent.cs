using System.Collections.Generic;
using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

[Board,Core,Input,SingleEntity]
public sealed class HolderComponent:IComponent
{
    public Dictionary<Res.InPools, Transform> poolDic;
}

