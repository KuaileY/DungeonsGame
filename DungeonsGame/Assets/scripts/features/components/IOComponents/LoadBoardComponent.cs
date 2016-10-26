using Entitas;
using Entitas.CodeGenerator;

[Board,SingleEntity]
public sealed class LoadBoardComponent:IComponent
{
    public int floor;
}

