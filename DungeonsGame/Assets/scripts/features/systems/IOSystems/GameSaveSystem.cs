using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

public sealed class GameSaveSystem:IReactiveSystem,ISetPools
{
    public TriggerOnEvent trigger { get { return InputMatcher.Save.OnEntityAdded(); } }
    Pools _pools;
    Dictionary<TileType, char> _tileTypeChar = new Dictionary<TileType, char>();

    public void SetPools(Pools pools)
    {
        _pools = pools;
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
        XmlDocument levelXml = SaveLevelData(LevelData.grids);
        XmlDocument itemsXml = SaveItemsData();

        formatXml(levelXml, path, Res.Files.levelData.ToString());
        formatXml(itemsXml, path, Res.Files.itemsData.ToString());

    }

    void formatXml(XmlDocument xdoc, StringBuilder path,string name)
    {
        StringWriter sw = new StringWriter();
        using (XmlTextWriter writer = new XmlTextWriter(sw))
        {
            writer.Indentation = 2;
            writer.Formatting = Formatting.Indented;
            xdoc.WriteContentTo(writer);
            writer.Close();
        }
        string data = sw.ToString();
        string file = path.ToString() + name;
        SaveElement(file, data);
    }

    XmlDocument SaveItemsData()
    {
        XmlDocument xdoc = createXML();
        return xdoc;
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
                roomE.SetAttribute("dirX", room.dir.x.ToString());
                roomE.SetAttribute("dirY",room.dir.y.ToString());

                XmlElement baseFloorE = (XmlElement)roomE.AppendChild(xdoc.CreateElement("baseFloor"));
                XmlElement dataE = (XmlElement) roomE.AppendChild(xdoc.CreateElement("data"));
                XmlElement itemsE = (XmlElement) roomE.AppendChild(xdoc.CreateElement("items"));
                
                baseFloorE.SetAttribute("encoding", "csv");
                dataE.SetAttribute("encoding", "csv");
                #region baseFloorE
                for (int k = 0; k < room.data.Length; k++)
                {
                    if (k % room.width == 0)
                    {
                        baseFloorE.InnerText += "\r\n";
                    }
                    baseFloorE.InnerText += room.data[k] + ",";
                }
                #endregion
                #region dataE
                for (int y = 0; y < room.height; y++)
                {
                    dataE.InnerText += "\r\n";
                    for (int x = 0; x < room.width; x++)
                    {
                        int _ = (int) room.grid[x, y];
                        dataE.InnerText += Res.TileTypeChar[_];
                    }
                }
                #endregion
                #region itemsE
                var items = _pools.core.dungeonItemsCache;
                for (int yy = 0; yy < room.height-2; yy++)
                {
                    for (int xx = 0; xx < room.width-1; xx++)
                    {
                        if (items.roomList[room.id - 1][xx, yy] != null)
                            itemsE.InnerText += string.Format("{0},{1},{2},{3}", room.id, xx, yy, 1111) + '|';
                    }
                }
                
                #endregion
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

