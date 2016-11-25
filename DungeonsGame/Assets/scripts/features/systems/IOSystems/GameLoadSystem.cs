using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Entitas;
public sealed class GameLoadSystem : ISetPools, IReactiveSystem
{
    public TriggerOnEvent trigger { get { return InputMatcher.Load.OnEntityAdded(); } }
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
    }

    public void Execute(List<Entity> entities)
    {
        entities.SingleEntity().IsDestroy(true);
        if(Load())
            _pools.board.CreateEntity().AddLoadBoard(1).AddPool(Res.InPools.Board);
    }

    bool Load()
    {
        StringBuilder path = new StringBuilder(Res.PathURL);
        path.Append("\\save\\");
        if (!Directory.Exists(path.ToString()))
            return false;
        else
        {
            List<string> fileList = new List<string>();
            fileList.Add(path.ToString() + Res.Files.levelData+".dat");

            bool fileNotFound = false;
            foreach (string s in fileList)
            {
                if (!File.Exists(s))
                    fileNotFound = true;
            }
            if (fileNotFound)
                return false;
            else
            {
                string levelData = LoadElement(path.ToString(), Res.Files.levelData.ToString());
                XDocument xdoc = XDocument.Parse(levelData);
                //_pools.input.fileList.fileDic.Add("levelData", xdoc);
                return true;
            }

        }
    }

    string LoadElement(string appPath, string filename)
    {
        string dataFile = appPath + filename + ".dat";
        string dataFileTemp = appPath + filename + ".tmp";
        FileInfo fiData = new FileInfo(dataFile);
        Decompress(fiData);
        FileInfo fiDataTemp = new FileInfo(dataFileTemp);

        StringBuilder dataBuffer = new StringBuilder(string.Empty);
        using (FileStream fsData = File.OpenRead(dataFileTemp))
        {
            byte[] bData = new byte[1024];
            UTF8Encoding tempUTFEncodingData = new UTF8Encoding(true);
            while (fsData.Read(bData, 0, bData.Length) > 0)
            {
                dataBuffer.Append(tempUTFEncodingData.GetString(bData).Trim('\0'));
                for (int i = 0; i < 1024; i++)
                {
                    bData[i] = Convert.ToByte('\0');
                }

            }
        }
        fiDataTemp.Delete();
        return dataBuffer.ToString().Trim('\0');
    }

    static void Decompress(FileInfo fi)
    {
        using (FileStream inFile = fi.OpenRead())
        {
            string curFile = fi.FullName;
            string origName = curFile.Remove(curFile.Length - fi.Extension.Length);
            origName = origName + ".tmp";
            using (FileStream outFile = File.Create(origName))
            {
//                 using (GZipStream Decompress = new GZipStream(inFile, CompressionMode.Decompress))
//                     Decompress.CopyTo(outFile);
                inFile.CopyTo(outFile);
            }
        }
    }

}

