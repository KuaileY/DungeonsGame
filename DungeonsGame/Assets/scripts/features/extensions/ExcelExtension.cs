
using System.IO;
using System.Xml;
using Entitas;
using OfficeOpenXml;
using UnityEngine;


public static class ExcelExtension
{
    public static void writeExcel(string outputDir)
    {
        FileInfo newFile = new FileInfo(outputDir);
        if (newFile.Exists)
        {
            newFile.Delete();  // ensures we create a new workbook  
            newFile = new FileInfo(outputDir);
        }
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            // add a new worksheet to the empty workbook  
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
            //Add the headers  
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Product";
            worksheet.Cells[1, 3].Value = "Quantity";
            worksheet.Cells[1, 4].Value = "Price";
            worksheet.Cells[1, 5].Value = "Value";

            //Add some items...  
            worksheet.Cells["A2"].Value = 12001;
            worksheet.Cells["B2"].Value = "Nails";
            worksheet.Cells["C2"].Value = 37;
            worksheet.Cells["D2"].Value = 3.99;

            worksheet.Cells["A3"].Value = 12002;
            worksheet.Cells["B3"].Value = "Hammer";
            worksheet.Cells["C3"].Value = 5;
            worksheet.Cells["D3"].Value = 12.10;

            worksheet.Cells["A4"].Value = 12003;
            worksheet.Cells["B4"].Value = "Saw";
            worksheet.Cells["C4"].Value = 12;
            worksheet.Cells["D4"].Value = 15.37;

            //save our new workbook and we are done!  
            package.Save();
        }
    }

    public static void readExcel(string file,Pool pool)
    {
        XmlDocument xdoc = new XmlDocument();
        string[] names = file.Split('/');
        string name = names[names.Length - 1].Split('.')[0];

        var root = xdoc.AppendChild(xdoc.CreateElement(name));
        var bytes = File.ReadAllBytes(file);
        var workbook = GetWorkbook(bytes);
        for (int i = 0; i < workbook.NumberOfSheets; i++)
        {
            var sheet= workbook.GetSheetAt(i);
            var node1 = root.AppendChild(xdoc.CreateElement(sheet.SheetName));

            var titles = sheet.GetRow(0);
            var types = sheet.GetRow(1);
            int rows = sheet.LastRowNum;
            int columns = titles.LastCellNum;
            for (int j = 1; j <= rows; j++)
            {
                if (sheet.GetRow(j).GetCell(0).ToString() == "")
                    break;
                XmlElement node2 = (XmlElement)node1.AppendChild(xdoc.CreateElement(sheet.GetRow(j).GetCell(1).ToString()));
                for (int k = 0; k < columns; k++)
                {
                    if (sheet.GetRow(j).GetCell(k).ToString() == "")
                        break;
                    node2.SetAttribute(titles.GetCell(k).ToString(),sheet.GetRow(j).GetCell(k).ToString());
                }
            }
        }

        pool.fileList.fileDic.Add(name, xdoc);

        //         var row = sheet.GetRow(0);
        //         var cell = row.GetCell(0);
        //         cell.SetCellType(NPOI.SS.UserModel.CellType.String);
        //         var txt = cell.StringCellValue;
        //         Debug.Log(txt);


    }

    static NPOI.SS.UserModel.IWorkbook GetWorkbook(byte[] bytes)
    {
        using (var mem = new MemoryStream(bytes))
        {
            return new NPOI.XSSF.UserModel.XSSFWorkbook(mem);
        }
    }

}

