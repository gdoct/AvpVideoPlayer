using System;
using System.Drawing;

namespace AvpVideoPlayer.Video.Snapshot;

public class SnapshotData : IDisposable
{
    public SnapshotData(Bitmap bmp, TimeSpan time, int index)
    {
        Bitmap = bmp;
        Timestamp = time;
        Index = index;
    }

    public Bitmap Bitmap { get; }

    public TimeSpan Timestamp { get; }


    public int Index { get; }
    
    public void Dispose()
    {
        Bitmap?.Dispose();
        GC.SuppressFinalize(this);
    }
}