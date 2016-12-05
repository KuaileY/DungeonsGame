using Entitas;
using Entitas.CodeGenerator;

[Input,SingleEntity]
public sealed class InputComponent:IComponent
{
    public string name;
    public int x;
    public int y;
}
