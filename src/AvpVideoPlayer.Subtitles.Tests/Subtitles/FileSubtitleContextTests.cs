namespace AvpVideoPlayer.Subtitles.Tests;

using AvpVideoPlayer.Subtitles;
using System;
using Xunit;

public class FileSubtitleContextTests : IDisposable
{


    private readonly FileSubtitleContext _testClass;
    private readonly TempSubtitle _subtitle;
    private bool disposedValue;

    public FileSubtitleContextTests()
    {
        _subtitle = TempSubtitle.Create();
        _testClass = new FileSubtitleContext(_subtitle.Filename);
    }

    [Fact]
    public void CanConstruct()
    {
        var instance = new FileSubtitleContext(_subtitle.Filename);
        Assert.NotNull(instance);
    }


    [Fact]
    public void CanCallGetSubtitleForTime()
    {
        var time = new TimeSpan(0, 0, 8);
        var result = _testClass.GetSubtitleForTime(time);
        Assert.NotNull(result);
    }

    [Fact]
    public void CanCallGetNextSubtitleForTime()
    {
        var time = new TimeSpan(0, 0, 8);
        var result = _testClass.GetNextSubtitleForTime(time);
        Assert.NotNull(result);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _subtitle.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~FileSubtitleContextTests()
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
