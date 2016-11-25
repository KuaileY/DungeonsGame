using Entitas;
using Entitas.CodeGenerator;
using System.Xml.Linq;

[Input,SingleEntity]
public sealed class SaveComponent:IComponent
{
    public string name;
    public XDocument xDoc;
}

