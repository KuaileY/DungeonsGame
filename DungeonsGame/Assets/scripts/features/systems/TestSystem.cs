using System.IO;
using Entitas;
using OfficeOpenXml;
using UnityEngine;

public sealed class TestSystem : IInitializeSystem
{
    string _file = Application.dataPath + "/art/Resources/database/param1.xlsx";
    public void Initialize()
    {
        writeExcel(_file);
        readExcel(_file);
    }

    void writeExcel(string outputDir)
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

    void readExcel(string file)
    { 
        var bytes = File.ReadAllBytes(file);

        var workbook = GetWorkbook(bytes);
        var sheet = workbook.GetSheet("Sheet1");
        var row = sheet.GetRow(0);
        var cell = row.GetCell(0);
        cell.SetCellType(NPOI.SS.UserModel.CellType.String);
        var txt = cell.StringCellValue;
        Debug.Log(txt);
    }

    NPOI.SS.UserModel.IWorkbook GetWorkbook(byte[] bytes)
    {
        using (var mem = new MemoryStream(bytes))
        {
            return new NPOI.XSSF.UserModel.XSSFWorkbook(mem);
        }
    }

}

