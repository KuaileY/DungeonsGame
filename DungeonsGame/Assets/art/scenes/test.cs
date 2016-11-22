using System.Collections;
using System.Collections.Generic;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {

        IWorkbook hssfworkbook;
	    string path = "d:/test.xlsx";
        using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            hssfworkbook = WorkbookFactory.Create(file);
        }
        XSSFFormulaEvaluator eva = new XSSFFormulaEvaluator(hssfworkbook);
        ICell cell=hssfworkbook.GetSheet("Sheet1").GetRow(0).GetCell(2);
	    cell.NumericCellValue.ToString().print();
	    cell.CellType.ToString().print();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
