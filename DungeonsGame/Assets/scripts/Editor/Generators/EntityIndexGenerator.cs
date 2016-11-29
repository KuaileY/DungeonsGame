using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NPOI.SS.UserModel;
using UnityEngine;

namespace Entitas.CodeGenerator
{
    public class EntityIndexGenerator:IPoolCodeGenerator
    {
        enum configs
        {
            items,
            monster
        }
        const string CLASS_TEMPLATE = @"
public static class ObjectsIndeies{{
{0}
}}
";

        static string addIndices()
        {
            var code = string.Empty;
            var arrays = string.Empty;
            const string fieldFormat = "    public const string {0}=\"{1}\";\n";
            var names = new List<string>();
            var allValues = (configs[])Enum.GetValues(typeof(configs));
            foreach (var value in allValues)
            {
                names.AddRange(readExcel(value.ToString()));
            }
            foreach (var name in names)
            {
                code += string.Format(fieldFormat, name, name);
                arrays += string.Format("        \"{0}\",\n", name);
            }

            if (arrays.EndsWith(",\n", System.StringComparison.Ordinal))
                arrays = arrays.Remove(arrays.Length - 2) + "\n";

            code += string.Format(@"    public static readonly string[] indexNames={{
{0}}};", arrays);

            return code;
        }



        public static List<string> readExcel(string file)
        {
            string path = Res.configPath + file + Res.xlsxExtension;
            var bytes = File.ReadAllBytes(path);
            IWorkbook workbook = GetWorkbook(bytes);
            //只读取第一页
            var sheet = workbook.GetSheetAt(0);
            var names = new List<string>();
            for (int i = 3; i <= sheet.LastRowNum; i++)
            {
                if (sheet.GetRow(i).GetCell(0).ToString() != string.Empty)
                    names.Add(sheet.GetRow(i).GetCell(1).StringCellValue);
            }
            return names;
        }

        static NPOI.SS.UserModel.IWorkbook GetWorkbook(byte[] bytes)
        {
            using (var mem = new MemoryStream(bytes))
            {
                return new NPOI.XSSF.UserModel.XSSFWorkbook(mem);
            }
        }

        public CodeGenFile[] Generate(string[] poolNames)
        {
            var aa = string.Format(CLASS_TEMPLATE, addIndices());
            return new[] {new CodeGenFile("ObjectsIndeiesExtension",aa,GetType().FullName) };
        }
    }
}
