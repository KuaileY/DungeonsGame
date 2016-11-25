using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Entitas;
using UniRx;
using UnityEngine;

public class GameSaveSystem:IReactiveSystem
{
    public TriggerOnEvent trigger { get { return InputMatcher.Save.OnEntityAdded(); } }
    public void Execute(List<Entity> entities)
    {
        foreach (var entity in entities)
        {
            IOExtension.Save(entity.save.xDoc,entity.save.name);
            entity.IsDestroy(true);
        }
    }

}

