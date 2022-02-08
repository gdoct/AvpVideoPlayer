
namespace AvpVideoPlayer.Video.Snapshot
{
    public interface ISnapshotGenerator
    {
        IEnumerable<SnapshotData> GenerateThumbnails(string inputPath, int amount);
        SnapshotData GenerateForTime(string inputPath, TimeSpan time);
    }
}