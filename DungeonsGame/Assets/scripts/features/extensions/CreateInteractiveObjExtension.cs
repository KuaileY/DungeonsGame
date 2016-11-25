

using System;
using Entitas;
using UniRx;
using UnityEngine;

public static class CreateInteractiveObjExtension
{
    public static void CreateObj(Pools pools, string name, int floor, int roomId, Vector2 pos = default(Vector2))
    {
        var db = pools.input.runtimeData.db;
        var configItem = db.Table<ConfigItem>();
        var typeLine = configItem.ElementAt(0);

        var entity = pools.core.CreateEntity();
        configItem.ToObservable()
            .Where(x => x.Name == name)
            .Do(x =>
            {
                configItem.Table.Columns.ToObservable()
                    .Where(y => y.Name != "ID"&&y.Name!="Name")
                    .Do(y =>
                    {
                        var value = x.GetType().GetProperty(y.Name).GetValue(x, null).ToString();
                        var type = typeLine.GetType().GetProperty(y.Name).GetValue(typeLine, null).ToString();
                        if (value != "nul")
                            CreateComponent(entity, y.Name, value, type);
                    })
                    .Subscribe();
            })
            .Subscribe();

    }

    private static void CreateComponent(Entity entity,string name,string value,string valueType)
    {
        int index = -1;
        for (int i = 0; i < CoreComponentIds.componentNames.Length; i++)
        {
            if (CoreComponentIds.componentNames[i] == name)
            {
                index = i;
                break;
            }
        }
        if (index == -1)
            throw new Exception("CoreComponentIds Index is null!");
        var type= CoreComponentIds.componentTypes[index];
        var component = entity.CreateComponent(index, type);
        int count = 0;
        foreach (var fi in component.GetType().GetFields())
        {
            fi.SetValue(component, Value(count, value, valueType));
            count++;
        }
        entity.AddComponent(index, component);
    }

    static object Value(int i, string value,string valueType)
    {
        string[] types = valueType.Split(',');
        string[] values = value.Split(',');
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
            case "bool":
                if (int.Parse(values[i]) == 1)
                    return true;
                else
                    return false;
            default:
                throw new Exception("setValue is wrong!");
        }
        return null;
    }

    public class ConfigItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Room { get; set; }
        public string Position { get; set; }
        public string Interactive { get; set; }
        public string Dir { get; set; }
        public string Pool { get; set; }
        public string Asset { get; set; }
        public string Controlable { get; set; }

    }
}

