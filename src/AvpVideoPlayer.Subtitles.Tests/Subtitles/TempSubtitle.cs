namespace AvpVideoPlayer.Subtitles.Tests;

/// <summary>
/// this class is disposable because auto-deleting the temp file 
/// in the disposable method is very convenient
/// </summary>
internal class TempSubtitle : IDisposable
{
    private const string sub = @"1
00:00:02,204 --> 00:00:04,375
It seems today
that all you see

2
00:00:04,442 --> 00:00:07,582
Is violence in movies
and sex on TV

3
00:00:07,649 --> 00:00:11,289
But where are those
good old?fashioned values

4
00:00:11,356 --> 00:00:14,295
On which we used to rely?";
    private readonly string _filename;
    private bool _disposedValue;

    private TempSubtitle()
    {
        _filename = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.srt");
        File.WriteAllText(_filename, sub);
    }
    public static TempSubtitle Create()
    {
        return new TempSubtitle();
    }

    private void Delete()
    {
        if (File.Exists(_filename))
        {
            File.Delete(_filename);
        }
    }
    public string Filename => _filename;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Delete();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~TempSubtitle()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
