using System.Collections.Generic;
using System.Xml;
using Entitas;
using UnityEngine;

public sealed class LoadXMLSystem:IReactiveSystem,ISetPool
{
    public TriggerOnEvent trigger { get { return InputMatcher.XML.OnEntityAdded(); } }
    Pool _pool;
    public void SetPool(Pool pool)
    {
        _pool = pool;
    }
    public void Execute(List<Entity> entities)
    {
        foreach (var e in entities)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(e.xML.name);
            var _roomsXml = new XmlDocument();
            _roomsXml.LoadXml(textAsset.text);
            _pool.fileList.name.Add(e.xML.name, _roomsXml);
            e.isDestroy = true;
        }
    }


}

