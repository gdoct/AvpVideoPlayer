using System.Runtime.InteropServices;
using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.Wpf.Logic;

// https://stackoverflow.com/a/4965166
public class IdleTimeDetector : IIdleTimeDetector
{
    public IdleTimeInfo GetIdleTimeInfo()
    {
        int systemUptime = Environment.TickCount,
            lastInputTicks,
            idleTicks = 0;

        LASTINPUTINFO lastInputInfo = new();
        lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
        lastInputInfo.dwTime = 0;

        if (GetLastInputInfo(ref lastInputInfo))
        {
            lastInputTicks = (int)lastInputInfo.dwTime;

            idleTicks = systemUptime - lastInputTicks;
        }

        return new IdleTimeInfo
        {
            LastInputTime = DateTime.Now.AddMilliseconds(-1 * idleTicks),
            IdleTime = new TimeSpan(0, 0, 0, 0, idleTicks),
            SystemUptimeMilliseconds = systemUptime,
        };
    }
    [DllImport("user32.dll")]
    private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
}

internal struct LASTINPUTINFO
{
    public uint cbSize;
    public uint dwTime;
}
