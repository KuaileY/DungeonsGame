using System;
using System.IO;

static class StreamExtension
{
    public static void CopyTo(this Stream source,Stream output)
    {
        byte[] buffer = new byte[16*1024];
        int bytesRead;
        while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, bytesRead);
        }
    }


}

