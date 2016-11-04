using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Entitas;
using UnityEngine;

public sealed class WatchDataSystem:IReactiveSystem,ISetPool
{
    public TriggerOnEvent trigger { get { return InputMatcher.Watch.OnEntityAdded(); } }
    Pool _pool;
    public void SetPool(Pool pool)
    {
        _pool = pool;
    }
    public void Execute(List<Entity> entities)
    {
        _pool.DestroyEntity(_pool.watchEntity);

        string path = Res.dataPath + "/art/Resources/watchData/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        foreach (KeyValuePair<string, XmlDocument> keyValuePair in _pool.fileList.fileDic)
        {
            var name = keyValuePair.Key.Split('/')[keyValuePair.Key.Split('/').Length - 1];
            XmlDocument xmlFile = _pool.fileList.fileDic[keyValuePair.Key];
            xmlFile.Save(path+name+".xml");
        }
        

    }

}

