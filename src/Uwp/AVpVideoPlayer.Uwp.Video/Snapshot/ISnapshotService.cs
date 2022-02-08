using System;
using Windows.UI.Xaml.Media;

namespace AvpVideoPlayer.Video.Snapshot;

public interface ISnapshotService
{
    ImageSource GetBitmapForTime(TimeSpan time);
    void LoadVideofile(string path);
}
