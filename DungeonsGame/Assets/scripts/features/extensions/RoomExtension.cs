
using System.Xml;
using Entitas;

public static class RoomExtension
{
    public static void setWH(out int width, out int height, string roomName,XmlDocument xdoc)
    {
        XmlNode xn = xdoc.SelectSingleNode("rooms");
        foreach (var node in xn.ChildNodes)
        {
            XmlElement xe = (XmlElement)node;
            if (xe.GetAttribute("name") == roomName)
            {
                width = int.Parse(xe.GetAttribute("width"));
                height = int.Parse(xe.GetAttribute("height"));
                return;
            }
        }
        throw new System.Exception("RoomExtension setWH is wrong!");
    }
}

