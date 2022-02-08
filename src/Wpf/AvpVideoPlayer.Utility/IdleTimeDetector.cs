using System.Runtime.InteropServices;

namespace AvpVideoPlayer.Utility;

// https://stackoverflow.com/a/4965166
public static class IdleTimeDetector
{
    public static IdleTimeInfo GetIdleTimeInfo()
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

public class IdleTimeInfo
{
    public TimeSpan IdleTime { get; internal set; }

    public DateTime LastInputTime { get; internal set; }

    public int SystemUptimeMilliseconds { get; internal set; }
}

internal struct LASTINPUTINFO
{
    public uint cbSize;
    public uint dwTime;
}
