using System;

public static class StringExtension
{
    public static Res.InPools GetInPools(string name)
    {
        return (Res.InPools) Enum.Parse(typeof (Res.InPools), name);
    }
}

