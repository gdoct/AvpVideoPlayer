namespace AvpVideoPlayer.Api
{
    public interface IIdleTimeDetector
    {
        IdleTimeInfo GetIdleTimeInfo();
    }

    public class IdleTimeInfo
    {
        public TimeSpan IdleTime { get; init; }

        public DateTime LastInputTime { get; init; }

        public int SystemUptimeMilliseconds { get; init; }
    }
}