﻿using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;

public class SSTimer : IDisposable
{
    public SSTimer(string tag)
    {
        _tag = tag;
        _stopWatch = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        _stopWatch.Stop();
        UnityEngine.Debug.LogFormat("'{0}' exec time: {1:0.000} (ms)", _tag,
            ((double) _stopWatch.ElapsedTicks/(double) Stopwatch.Frequency)*1000);
    }

    string _tag;
    Stopwatch _stopWatch;
}
/*
 * using (SSTimer t = new SSTimer("_name_tag_"))
    {
        // test code 
        foo();
    }
 * */
