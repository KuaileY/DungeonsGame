using System.Collections.Generic;
using Entitas;
using Entitas.CodeGenerator;

[Core,SingleEntity]
public sealed class DungeonItemsCacheComponent : IComponent
{
    public List<Entity[,]> roomList;
}

