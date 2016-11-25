
using System;
using System.IO;
using System.Xml;
using Entitas;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using SQLite4Unity3d;
using UniRx;
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

    public static void readExcel(string file, Pool pool, SQLiteConnection db)
    {
        var bytes = File.ReadAllBytes(file);
        IWorkbook workbook = GetWorkbook(bytes);
        //只读取第一页
        var sheet = workbook.GetSheetAt(0);
        var name = "config" + sheet.SheetName;
        //删除原始表
        db.Execute(string.Format("drop table if exists {0}", name));
        //创建表
        sheet.GetRow(0).ToObservable()
            .Zip(sheet.GetRow(1).ToObservable(), (lhs, rhs) => string.Format("{0} {1}", lhs, rhs))
            .ToArray<string>()
            .Subscribe(xx =>
            {
                var query = string.Format("create table {0} ( {1} );", name,
                    xx.ConvertStringArrayToString(Res.xlsType.title));
                db.Execute(query);
            });
        //插入数据
        db.BeginTransaction();
        Observable.Range(2, sheet.LastRowNum - 1)
            .Where(x => sheet.GetRow(x).GetCell(0).ToString() != string.Empty)
            .Select(x => sheet.GetRow(x))
            .Do(x =>
            {
                x.ToObservable()
                    .Select(y=> GetCellValue(y))
                    .ToArray<string>()
                    .Subscribe(yy =>
                    {
                        var query = string.Format("insert into {0} values({1});", name,
                            yy.ConvertStringArrayToString(Res.xlsType.data));
                        db.Execute(query);
                    });
            })
            .Subscribe();
        db.Commit();

    }

    static string GetCellValue(ICell cell)
    {
        string value = String.Empty;
        try
        {
            if (cell.CellType != CellType.Blank)
            {
                switch (cell.CellType)
                {
                    case CellType.Numeric:
                        // Date comes here
                        if (DateUtil.IsCellDateFormatted(cell))
                            value = cell.DateCellValue.ToString();
                        else
                            value = cell.NumericCellValue.ToString();
                        break;
                    case CellType.Boolean:
                        // Boolean type
                        value = cell.BooleanCellValue.ToString();
                        break;
                        //公式
                    case CellType.Formula:
                        if (cell.CachedFormulaResultType == CellType.String)
                            value = cell.StringCellValue;
                        else
                            value = cell.NumericCellValue.ToString();
                        break;
                    default:
                        // String type
                        value = cell.StringCellValue;
                        break;
                }
            }
        }
        catch (Exception)
        {
            value = "";
        }

        return value;
    }

    static NPOI.SS.UserModel.IWorkbook GetWorkbook(byte[] bytes)
    {
        using (var mem = new MemoryStream(bytes))
        {
            return new NPOI.XSSF.UserModel.XSSFWorkbook(mem);
        }
    }

}

