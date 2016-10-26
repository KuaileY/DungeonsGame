using Entitas;
using Entitas.CodeGenerator;

[Core, SingleEntity]
public sealed class DungeonComponent : IComponent
{
    public int value;
}
