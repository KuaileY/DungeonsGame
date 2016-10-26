using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

public sealed class GameSaveSystem:IReactiveSystem,ISetPool
{
    public TriggerOnEvent trigger { get { return InputMatcher.Save.OnEntityAdded(); } }
    Pool _pool;
    Dictionary<TileType, char> _tileTypeChar = new Dictionary<TileType, char>();

    public void SetPool(Pool pool)
    {
        _pool = pool;
    }
    public void Execute(List<Entity> entities)
    {
        entities.SingleEntity().IsDestroy(true);
        Save();
    }

    void Save()
    {
        StringBuilder path = new StringBuilder(Res.PathURL);
        path.Append("\\save\\");
        if (!Directory.Exists(path.ToString()))
        {
            Directory.CreateDirectory(path.ToString());
        }
        XmlDocument xdoc = SaveLevelData(LevelData.grids);

        StringWriter sw = new StringWriter();
        using (XmlTextWriter writer = new XmlTextWriter(sw))
        {
            writer.Indentation = 2;
            writer.Formatting = Formatting.Indented;
            xdoc.WriteContentTo(writer);
            writer.Close();
        }
        string levelData = sw.ToString();
        string mapFile = path.ToString() + Res.Files.levelData.ToString();
        SaveElement(mapFile, levelData);
    }

    XmlDocument SaveLevelData(List<SingleGrid> grids)
    {
        XmlDocument xdoc = createXML();
        XmlNode levelE = xdoc.AppendChild(xdoc.CreateElement("level"));
        int i = 0;
        foreach (var grid in grids)
        {
            XmlElement gridE = (XmlElement)levelE.AppendChild(xdoc.CreateElement("grid"));
            gridE.SetAttribute("name", grid.name);
            gridE.SetAttribute("width", grid.width.ToString());
            gridE.SetAttribute("height", grid.height.ToString());
            int j = 0;
            foreach (var room in grid.rooms)
            {
                XmlElement roomE = (XmlElement)gridE.AppendChild(xdoc.CreateElement("room"));
                roomE.SetAttribute("name", room.name);
                roomE.SetAttribute("id", j.ToString());
                roomE.SetAttribute("width", room.width.ToString());
                roomE.SetAttribute("height", room.height.ToString());
                roomE.SetAttribute("x", room.pos.x.ToString());
                roomE.SetAttribute("y", room.pos.y.ToString());

                XmlElement baseFloorE = (XmlElement)roomE.AppendChild(xdoc.CreateElement("baseFloor"));
                XmlElement dataE = (XmlElement) roomE.AppendChild(xdoc.CreateElement("data"));
                baseFloorE.SetAttribute("encoding", "csv");
                dataE.SetAttribute("encoding", "csv");
                for (int k = 0; k < room.data.Length; k++)
                {
                    if (k % room.width == 0)
                    {
                        baseFloorE.InnerText += "\r\n";
                    }
                    baseFloorE.InnerText += room.data[k] + ",";
                }

                for (int y = 0; y < room.height; y++)
                {
                    dataE.InnerText += "\r\n";
                    for (int x = 0; x < room.width; x++)
                    {
                        int _ = (int) room.grid[x, y];
                        dataE.InnerText += Res.TileTypeChar[_];
                    }
                }
                j++;
            }
            i++;
        }
        return xdoc;
    }

    #region 通用方法
    void SaveElement(string filename, string data)
    {
        using (FileStream fs = File.Create(filename))
        {
            Byte[] info = new UTF8Encoding(true).GetBytes(data);
            fs.Write(info, 0, info.Length);
        }
        FileInfo fi = new FileInfo(filename);
        Comptess(fi);
    }

    static void Comptess(FileInfo fi)
    {
        using (FileStream inFile = fi.OpenRead())
        {
            if ((File.GetAttributes(fi.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden &
                fi.Extension != ".dat")
            {
                using (FileStream outFile = File.Create(fi.FullName + ".dat"))
                {
                    //                     using (GZipStream compress = new GZipStream(outFile, CompressionMode.Compress))
                    //                         inFile.CopyTo(compress);
                    inFile.CopyTo(outFile);
                }
            }
        }
        fi.Delete();
    }

    XmlDocument createXML()
    {
        XmlDocument xml = new XmlDocument();
        xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
        return xml;
    }
    #endregion

}

