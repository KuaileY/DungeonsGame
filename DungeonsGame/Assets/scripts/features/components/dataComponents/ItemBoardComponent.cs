using System.Collections.Generic;
using Entitas;
using Entitas.CodeGenerator;

[Core,SingleEntity]
public sealed class ItemBoardComponent : IComponent
{
    public List<rect> roomList;

    public struct rect
    {
        public int columns;
        public int rows;
    } 
}

