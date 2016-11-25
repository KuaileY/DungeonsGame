using System.Collections.Generic;
using System.Xml.Linq;
using Entitas;
using Entitas.CodeGenerator;

[Board,SingleEntity]
public class GameBoardComponent:IComponent
{
    public int floor;
}

