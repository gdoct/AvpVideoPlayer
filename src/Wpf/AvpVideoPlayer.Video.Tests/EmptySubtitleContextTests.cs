namespace AvpVideoPlayer.Subtitles.Tests;

using AvpVideoPlayer.Video.Subtitles;
using System;
using Xunit;

public class EmptySubtitleContextTests
{
    private readonly EmptySubtitleContext _testClass;

    public EmptySubtitleContextTests()
    {
        _testClass = new EmptySubtitleContext();
    }

    [Fact]
    public void CanCallGetNextSubtitleForTime()
    {
        var time = new TimeSpan(0, 1, 0);

        var result = _testClass.GetNextSubtitleForTime(time);
        Assert.Null(result);
    }

    [Fact]
    public void CanCallGetSubtitleForTime()
    {
        var time = new TimeSpan();
        var result = _testClass.GetSubtitleForTime(time);
        Assert.Null(result);
    }
}
