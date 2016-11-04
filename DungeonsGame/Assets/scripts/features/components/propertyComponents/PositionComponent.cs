using Entitas;
using UnityEngine;

[Board,Core]
public sealed class PositionComponent:IComponent
{
    public int grid;
    public int roomId;
    public Vector3 value;
}

