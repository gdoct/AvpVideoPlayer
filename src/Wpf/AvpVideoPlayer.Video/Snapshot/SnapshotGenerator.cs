using FFMpegCore;
using System.Drawing;

namespace AvpVideoPlayer.Video.Snapshot;

internal class SnapshotGenerator : ISnapshotGenerator
{
    public SnapshotData GenerateForTime(string inputPath, TimeSpan time)
    {
        var bitmap = FFMpeg.Snapshot(inputPath, new Size(160, 90), time);
        return new SnapshotData(bitmap, time, 0);
    }

    public IEnumerable<SnapshotData> GenerateThumbnails(string inputPath, int amount)
    {
        var info = FFProbe.Analyse(inputPath);
        TimeSpan avg;
        if (info == null) yield break;
        else avg = info.Duration / (amount + 2);
        Size imagesize;
        if (info?.PrimaryVideoStream != null)
        {
            double ratio = info.PrimaryVideoStream.Width / (double) info.PrimaryVideoStream.Height;
            // 16/9
            imagesize = new Size((int)(ratio * 90), 90);
        }
        else
        {
            imagesize = new Size(160, 90);
        }
        // get frames in zigzag order
        var set = Enumerable.Range(0, amount).OrderBy(n => n%(amount /3));
        foreach(var i in set)
        {
            var timestamp = i * avg;
            var bitmap = FFMpeg.Snapshot(inputPath, imagesize, timestamp);
            yield return new SnapshotData(bitmap, timestamp, i);
        }
    }
}