using System.Collections.Generic;
using System.Xml;
using Entitas;
using Entitas.CodeGenerator;

[Input,SingleEntity]
public class FileListComponent:IComponent
{
    public Dictionary<string, XmlDocument> fileDic;
}

