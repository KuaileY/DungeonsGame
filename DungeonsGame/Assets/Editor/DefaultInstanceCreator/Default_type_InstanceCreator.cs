using System;
using Entitas.Unity.VisualDebugging;

// Please rename class name and file name
public class Default_type_InstanceCreator : IDefaultInstanceCreator {
    public bool HandlesType(Type type) {
        return type == typeof(IViewController);
    }

    public object CreateDefault(Type type) {
        // return your implementation to create an instance of type IViewController
        throw new NotImplementedException();
    }
}
