
using System;
using System.Collections.Generic;
using System.Xml;

public static class BoardExtension
{
    public static SingleGrid createGrid(int width,int height,string name)
    {
        var singleGrid = new SingleGrid();
        SingleGrid.Tile[,] grids = new SingleGrid.Tile[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grids[x, y].type = TileType.empty;
            }
        }
        singleGrid.grids = grids;
        singleGrid.width = width;
        singleGrid.height = height;
        singleGrid.rooms = new List<SingleRoom>();
        singleGrid.name = name;
        return singleGrid;
    }

    public static SingleGrid loadGrid(XmlElement xE)
    {
        var singleGrid = new SingleGrid();
        singleGrid.name = xE.GetAttribute("name");
        singleGrid.width = int.Parse(xE.GetAttribute("width"));
        singleGrid.height = int.Parse(xE.GetAttribute("height"));
        singleGrid.rooms = new List<global::SingleRoom>();
        foreach (var room in xE.ChildNodes)
        {
            var roomE = (XmlElement)room;
            var singleRoom = new SingleRoom();
            singleRoom.id= int.Parse(roomE.GetAttribute("id"));
            singleRoom.name = roomE.GetAttribute("name");
            singleRoom.width = int.Parse(roomE.GetAttribute("width"));
            singleRoom.height = int.Parse(roomE.GetAttribute("height"));
            int x = int.Parse(roomE.GetAttribute("x"));
            int y = int.Parse(roomE.GetAttribute("y"));
            singleRoom.pos = new UnityEngine.Vector2(x, y);
            singleRoom.tiles = new UnityEngine.GameObject[singleRoom.width, singleRoom.height];
            singleRoom.data = ParseData(roomE["baseFloor"], singleRoom.width, singleRoom.height);
            singleGrid.rooms.Add(singleRoom);
        }
        return singleGrid;
    }

    private static int[] ParseData(XmlElement e, int w, int h)
    {
        if (e.GetAttribute("encoding") == "csv")
        {
            return ParseCsvData(e.InnerText, w, h);
        }
        throw new Exception("Unsupported layer encoding - 'csv' only please");
    }

    private static int[] ParseCsvData(string s, int w, int h)
    {
        int[] gids = new int[w * h];
        int i = 0;
        if (s[s.Length - 1] == ',')
            s = s.Substring(0, s.Length - 1);
        foreach (var index in s.Split(','))
        {
            var gid = int.Parse(index.Trim());
            gids[i] = gid;
            i++;
        }
        return gids;
    }

}
