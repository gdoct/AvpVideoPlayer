using System;

namespace AvpVideoPlayer.Uwp.Utility;
public static class Logger
{
    public static void Log(this object _, string s)
    {
        System.Diagnostics.Debug.WriteLine($"[{DateTime.Now:T}] {s}");
    }
}
