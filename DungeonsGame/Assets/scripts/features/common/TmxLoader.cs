using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


public static class TmxLoader
{
    public static TileMap Parse(string fileName)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(fileName);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(textAsset.text);
        //Debug.Log(textAsset.text);
        return Parse(doc);
    }

    public static TileMap Parse(XmlDocument document)
    {
        return new TileMap(document.DocumentElement);
    }
}

public class TileMap
{
    public string Version { get; private set; }
    public string Orientation { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int TileWidth { get; private set; }
    public int TileHeight { get; private set; }
    public string BackgroundColor { get; private set; }
    public string RenderOrder { get; private set; }
    public List<TiledTileset> Tilesets { get; private set; }
    public List<TiledLayer> Layers { get; private set; }
    public List<TiledObjectGroup> ObjectGroups { get; private set; }
    public Dictionary<int, TiledTile> TilesByGid { get; private set; }

    public TileMap(XmlElement e)
    {
        ParseMap(e);
    }

    private void ParseMap(XmlElement e)
    {
        LoadAttributes(e);
        LoadTilesets(e);
        LoadLayers(e);
        LoadObjectGroups(e);
    }
    private void LoadAttributes(XmlElement e)
    {
        Version = e.GetAttribute("version");
        Orientation = e.GetAttribute("orientation");
        Width = int.Parse(e.GetAttribute("width"));
        Height = int.Parse(e.GetAttribute("height"));
        TileWidth = int.Parse(e.GetAttribute("tilewidth"));
        TileHeight = int.Parse(e.GetAttribute("tileheight"));
        BackgroundColor = e.GetAttribute("backgroundColor");
        RenderOrder = e.GetAttribute("renderorder");
    }

    private void LoadTilesets(XmlElement e)
    {
        TilesByGid = new Dictionary<int, TiledTile>();
        var tilesets = e.SelectNodes("tileset");
        Tilesets = new List<TiledTileset>();
        foreach (var tileset in tilesets)
        {
            var tmxTileset = new TiledTileset(tileset as XmlElement);
            Tilesets.Add(tmxTileset);
            foreach (var tile in tmxTileset.Tiles)
            {
                TilesByGid[tile.Id + tmxTileset.FirstGid] = tile;
            }
        }
    }

    private void LoadLayers(XmlElement e)
    {
        var layers = e.SelectNodes("layer");
        Layers = new List<TiledLayer>();
        foreach (var layer in layers)
        {
            Layers.Add(new TiledLayer(layer as XmlElement));
        }
    }

    private void LoadObjectGroups(XmlElement e)
    {
        var objectGroups = e.SelectNodes("objectgroup");
        ObjectGroups = new List<TiledObjectGroup>();
        foreach (var objectGroup in objectGroups)
        {
            ObjectGroups.Add(new TiledObjectGroup(objectGroup as XmlElement));
        }
    }

    public int TilesetIndex(int gid)
    {
        for (var i = Tilesets.Count - 1; i >= 0; i--)
        {
            if (gid >= Tilesets[i].FirstGid)
            {
                return i;
            }
        }
        return -1;
    }
}

public class TiledTileset
{
    public int FirstGid { get; private set; }

    public string Source { get; private set; }

    public string Name { get; private set; }

    public int TileWidth { get; private set; }

    public int TileHeight { get; private set; }

    public int Spacing { get; private set; }

    public int Margin { get; private set; }

    public TiledImage Image { get; private set; }
    public List<TiledTile> Tiles { get; private set; }
    public TiledTileset(XmlElement e)
    {
        ParseTileset(e);
    }

    void ParseTileset(XmlElement e)
    {
        FirstGid = int.Parse(e.GetAttribute("firstgid"));
        Source = e.GetAttribute("source");
        Name = e.GetAttribute("name");
        TileWidth = int.Parse(e.GetAttribute("tilewidth"));
        TileHeight = int.Parse(e.GetAttribute("tileheight"));

        Spacing = e.HasAttribute("spacing")
            ? int.Parse(e.GetAttribute("spacing"))
            : 0;

        Margin = e.HasAttribute("margin")
            ? int.Parse(e.GetAttribute("margin"))
            : 0;

        if (e["image"] != null)
        {
            Image = new TiledImage(e["image"]);
        }

        Tiles = new List<TiledTile>();
        var tiles = e.SelectNodes("tile");
        foreach (var tile in tiles)
        {
            Tiles.Add(new TiledTile(tile as XmlElement));
        }
    }
}

public class TiledTile
{
    public int Id { get; private set; }

    public List<TiledProperty> Properties { get; private set; }

    public TiledTile(XmlElement e)
    {
        ParseTile(e);
    }

    private void ParseTile(XmlElement e)
    {
        Id = int.Parse(e.GetAttribute("id"));

        Properties = new List<TiledProperty>();
        if (e["properties"] != null)
        {
            var ep = e["properties"];
            var properties = ep.SelectNodes("property");
            foreach (var property in properties)
            {
                Properties.Add(new TiledProperty(property as XmlElement));
            }
        }
    }

    public string GetPropertyByName(string name)
    {
        TiledProperty property = Properties.Find(e => e.Name == name);
        return property != null ? property.Value : null;
    }
}

public class TiledProperty
{
    public string Name { get; private set; }

    public string Value { get; private set; }

    public TiledProperty(XmlElement e)
    {
        ParseProperty(e);
    }

    private void ParseProperty(XmlElement e)
    {
        Name = e.GetAttribute("name");
        Value = e.GetAttribute("value");
    }
}

public class TiledImage
{
    public string Source { get; private set; }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public TiledImage(XmlElement e)
    {
        ParseImage(e);
    }

    private void ParseImage(XmlElement e)
    {
        Source = e.GetAttribute("source");
        Width = int.Parse(e.GetAttribute("width"));
        Height = int.Parse(e.GetAttribute("height"));
    }
}

public class TiledLayer
{
    public string Name { get; private set; }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public float Opacity { get; private set; }

    public int[] Data { get; private set; }

    public bool Visible { get; private set; }

    public TiledLayer(XmlElement e)
    {
        ParseLayer(e);
    }

    private void ParseLayer(XmlElement e)
    {
        Name = e.GetAttribute("name");
        Width = int.Parse(e.GetAttribute("width"));
        Height = int.Parse(e.GetAttribute("height"));
        Opacity = e.HasAttribute("opacity")
            ? float.Parse(e.GetAttribute("opacity"))
            : 1.0f;
        Visible = !e.HasAttribute("visible") || int.Parse(e.GetAttribute("visible")) != 0;

        Data = ParseData(e["data"]);
    }

    private int[] ParseData(XmlElement e)
    {
        if (e.GetAttribute("encoding") == "csv")
        {
            return ParseCsvData(e.InnerText);
        }
        throw new Exception("Unsupported layer encoding - 'csv' only please");
    }

    private int[] ParseCsvData(string s)
    {
        int[] gids = new int[Width * Height];
        int i = 0;
        foreach (var index in s.Split(','))
        {
            var gid = int.Parse(index.Trim());
            gids[i] = gid;
            i++;
        }
        return gids;
    }
}

public class TiledObjectGroup
{
    public string Name { get; private set; }

    public string Color { get; private set; }

    public float Opacity { get; private set; }

    public bool Visible { get; private set; }

    public List<TiledObject> Objects { get; private set; }

    public TiledObjectGroup(XmlElement e)
    {
        ParseObjectGroup(e);
    }

    private void ParseObjectGroup(XmlElement e)
    {
        Name = e.GetAttribute("name");
        Color = e.GetAttribute("color");
        Opacity = e.HasAttribute("opacity")
            ? float.Parse(e.GetAttribute("opacity"))
            : 1.0f;
        Visible = !e.HasAttribute("visible") || int.Parse(e.GetAttribute("visible")) != 0;

        var objects = e.SelectNodes("object");
        Objects = new List<TiledObject>();
        foreach (var obj in objects)
        {
            Objects.Add(new TiledObject(obj as XmlElement));
        }
    }
}

public class TiledObject
{
    public int Id { get; private set; }

    public string Name { get; set; }

    public string Type { get; private set; }

    public int X { get; private set; }

    public int Y { get; private set; }

    public int Width { get; private set; }

    public int Height { get; private set; }

    public int Rotation { get; private set; }

    public int Gid { get; private set; }

    public bool Visible { get; private set; }

    public TiledObject(XmlElement e)
    {
        ParseObject(e);
    }

    private void ParseObject(XmlElement e)
    {
        Id = e.HasAttribute("id")
            ? int.Parse(e.GetAttribute("id"))
            : -1;
        Name = e.GetAttribute("name");
        Type = e.GetAttribute("type");
        X = int.Parse(e.GetAttribute("x"));
        Y = int.Parse(e.GetAttribute("y"));
        Width = e.HasAttribute("width")
            ? int.Parse(e.GetAttribute("width"))
            : 0;
        Height = e.HasAttribute("height")
            ? int.Parse(e.GetAttribute("height"))
            : 0;
        Rotation = e.HasAttribute("rotation")
            ? int.Parse(e.GetAttribute("rotation"))
            : 0;
        Gid = e.HasAttribute("gid")
            ? int.Parse(e.GetAttribute("gid"))
            : -1;
        Visible = !e.HasAttribute("visible") || int.Parse(e.GetAttribute("visible")) != 0;
    }
}