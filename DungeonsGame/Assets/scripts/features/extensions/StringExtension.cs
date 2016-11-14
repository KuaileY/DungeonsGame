using System;
using System.Text;
using System.Text.RegularExpressions;

public static class StringExtension
{
    public static Res.InPools GetInPools(string name)
    {
        return (Res.InPools) Enum.Parse(typeof (Res.InPools), name);
    }

    public static string ConvertStringArrayToString(this string[] array,Res.xlsType val)
    {
        StringBuilder builder = new StringBuilder();
        foreach (string s in array)
        {
            if (s == string.Empty || s == " ")
                break;

            switch (val)
            {
                case Res.xlsType.title:
                    builder.Append(s);
                    builder.Append(',');
                    break;
                case Res.xlsType.data:
                    if (s.IsType())
                    {
                        builder.Append(s);
                        builder.Append(',');
                    }
                    else
                    {
                        builder.Append('"');
                        builder.Append(s);
                        builder.Append('"');
                        builder.Append(',');
                    }
                    break;
            }
        }
        return builder.ToString().TrimEnd(',');
    }

    public static bool IsType(this string value)
    {
        return value == "int" || value == "text" || value == "null" || value == "real" || value == "blob";
    }

    public static bool IsNumeric(this string value)
    {
        return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
    }

    public static bool IsInt(this string value)
    {
        return Regex.IsMatch(value, @"^[+-]?\d*$");
    }

    public static bool IsUnsign(this string value)
    {
        return Regex.IsMatch(value, @"^\d*[.]?\d*$");
    }

    public static bool isTel(this string strInput)
    {
        return Regex.IsMatch(strInput, @"\d{3}-\d{8}|\d{4}-\d{7}");
    }
}

