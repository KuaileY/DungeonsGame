
using System.IO;
using System.Text;
using System.Xml.Linq;

public static class IOExtension
{
    public static void Save(XDocument Xdoc, string name)
    {
        StringBuilder path = new StringBuilder(Res.PathURL);
        path.Append("\\save\\");
        if (!Directory.Exists(path.ToString()))
        {
            Directory.CreateDirectory(path.ToString());
        }
        Xdoc.Save(path + name + "Data.dat");
    }

}

