using System.Drawing;

namespace AvpVideoPlayer.Video.Snapshot;

public class SnapshotData : IDisposable
{
    private bool disposedValue;

    public SnapshotData(Bitmap bmp, TimeSpan time, int index)
    {
        Bitmap = bmp;
        Timestamp = time;
        Index = index;
    }

    public Bitmap Bitmap { get; }

    public TimeSpan Timestamp { get; }


    public int Index { get; }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                Bitmap?.Dispose();
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}