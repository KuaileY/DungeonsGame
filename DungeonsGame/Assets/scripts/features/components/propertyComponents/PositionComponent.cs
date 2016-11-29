using Entitas;
using UnityEngine;

[Board,Core]
public sealed class PositionComponent:IComponent
{
    public int roomId;
    public Vector3 value;
}

