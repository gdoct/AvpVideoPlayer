using System.Windows.Media;

namespace AvpVideoPlayer.Video.Snapshot
{
    public interface ISnapshotService
    {
        ImageSource? GetBitmapForTime(TimeSpan time);
        void LoadVideofile(string path);
    }
}