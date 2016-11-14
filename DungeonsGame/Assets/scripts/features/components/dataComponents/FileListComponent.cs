using System.Collections.Generic;
using System.Xml.Linq;
using Entitas;
using Entitas.CodeGenerator;

[Input,SingleEntity]
public class FileListComponent:IComponent
{
    public Dictionary<string, XDocument> fileDic;
}

