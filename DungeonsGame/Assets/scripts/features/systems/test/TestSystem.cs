using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Entitas;
using OfficeOpenXml;
using UnityEngine;

public sealed class TestSystem : IInitializeSystem, IEntityCollectorSystem,ISetPools
{
    public EntityCollector entityCollector { get { return _entityCollector; } }
    string _file = Application.dataPath + "/art/Resources/database/param1.xlsx";
    EntityCollector _entityCollector;
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
        _entityCollector = new[] {pools.board, pools.core}
            .CreateEntityCollector(Matcher.AnyOf(BoardMatcher.Asset, CoreMatcher.Asset));
    }
    public void Initialize()
    {
        var key = Res.configs.items.ToString();
        var data = _pools.input.fileList.fileDic[key];
        XmlNode items = data.SelectSingleNode(key);
        XmlElement sheet = (XmlElement)items.FirstChild;
        //var row = (XmlElement)sheet.ChildNodes[1];
        //检测配置个数是否对于
        checkItem((XmlElement)sheet.FirstChild);

        Debug.Log("count:"+sheet.ChildNodes.Count);
        for (int j = 1; j < sheet.ChildNodes.Count; j++)
        {
            Entity entity = _pools.core.CreateEntity();
            int i = 0;
            foreach (XmlAttribute attribute in sheet.ChildNodes[j].Attributes)
            {
                if (i > 1)
                    createItem(entity, attribute, (XmlElement)sheet.FirstChild);
                i++;
            }
        }
        
    }

    void checkItem(XmlElement xe)
    {
        int i = 0;
        foreach (XmlAttribute attribute in xe.Attributes)
        {
            if (i > 1)
            {
                for (int j = 0; j < CoreComponentIds.componentNames.Length; j++)
                {
                    if (CoreComponentIds.componentNames[j] == attribute.Name)
                    {
                        int length;
                        if (attribute.Value == "trigger")
                            length = 0;
                        else
                            length = attribute.Value.Split(',').Length;
                        if (CoreComponentIds.componentTypes[j].GetFields().Length != length)
                            throw new Exception("checkItem is wrong");
                    }
                }
            }
            i++;
        }
    }

    public void Execute(List<Entity> entities)
    {
        //lala
    }

    void createItem(Entity entity, XmlAttribute attribute,XmlElement firstRow)
    {
        int index = -1;
        for (int i = 0; i < CoreComponentIds.componentNames.Length ; i++)
        {
            if (CoreComponentIds.componentNames[i] == attribute.Name)
            {
                index = i;
                break;
            }
        }
        if (index == -1)
            throw new Exception("createItem is wrong!");
        //没有设定的情况
        if (attribute.Value == "null")
            return;
        Type type = CoreComponentIds.componentTypes[index];
        IComponent component = entity.CreateComponent(index, type);
        int count = 0;
        foreach (var fi in component.GetType().GetFields())
        {
            fi.SetValue(component, value(count, attribute, firstRow));
            count++;
        }
        entity.AddComponent(index, component);
    }

    object value(int i, XmlAttribute attribute, XmlElement fristRow)
    {
        string[] types=fristRow.GetAttribute(attribute.Name).Split(',');
        string[] values = attribute.Value.Split(',');
        switch (types[i])
        {
            case "int":
                return int.Parse(values[i]);
            case "string":
                return values[i];
            case "vector2":
                string[] vec2 = values[i].Split('|');
                return new Vector2(int.Parse(vec2[0]), int.Parse(vec2[1]));
            case "vector3":
                string[] vec3 = values[i].Split('|');
                return new Vector3(int.Parse(vec3[0]), int.Parse(vec3[1]),int.Parse(vec3[2]));
            case "inPools":
                return (Res.InPools) Enum.Parse(typeof (Res.InPools), values[i]);
            default:
                throw new Exception("setValue is wrong!");
        }
    }

    void createEntity()
    {
        var entity = _pools.input.CreateEntity();
        Type type = InputComponentIds.componentTypes[10];
        //var hehe = entity.CreateComponent<type>(10);
        var component = entity.CreateComponent(10, type);
        System.Reflection.FieldInfo ff = component.GetType().GetField("value");
        ff.SetValue(component, (ulong)80);
        entity.AddComponent(10, component);
        Debug.Log(entity.tick.value);
    }

}

