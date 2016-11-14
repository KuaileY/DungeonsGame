using Entitas;
using Entitas.CodeGenerator;
using SQLite4Unity3d;

[Input,SingleEntity]
public class RuntimeDataComponent:IComponent
{
    public SQLiteConnection db;
}

