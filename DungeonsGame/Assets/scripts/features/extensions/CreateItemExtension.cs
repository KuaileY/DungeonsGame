
using System;
using System.Xml;
using Entitas;
using UnityEngine;

public static class CreateItemExtension
{
    static void createItem(Entity entity, XmlAttribute attribute, XmlElement firstRow)
    {
        int index = -1;
        for (int i = 0; i < CoreComponentIds.componentNames.Length; i++)
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

    static object value(int i, XmlAttribute attribute, XmlElement fristRow)
    {
        string[] types = fristRow.GetAttribute(attribute.Name).Split(',');
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
                return new Vector3(int.Parse(vec3[0]), int.Parse(vec3[1]), int.Parse(vec3[2]));
            case "inPools":
                return (Res.InPools)Enum.Parse(typeof(Res.InPools), values[i]);
            default:
                throw new Exception("setValue is wrong!");
        }
    }
}

