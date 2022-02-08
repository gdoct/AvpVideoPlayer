#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace AvpVideoPlayer.Subtitles.Tests;

using AvpVideoPlayer.Video.Subtitles;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class SubtitleServiceTests
{
    private readonly SubtitleService _testClass;
    private readonly ISubtitleContextFactory _subtitleContextFactory;

    public SubtitleServiceTests()
    {
        _subtitleContextFactory = Mock.Of<ISubtitleContextFactory>();
        _testClass = new SubtitleService(_subtitleContextFactory);
    }

    [Fact]
    public void CanConstruct()
    {
        var instance = new SubtitleService(_subtitleContextFactory);
        Assert.NotNull(instance);
    }

    [Fact]
    public void CannotConstructWithNullSubtitleContextFactory()
    {
        Assert.Throws<ArgumentNullException>(() => new SubtitleService(default));
    }

    [Fact]
    public void CanCallAddSubtitlesFromFile()
    {
        var instance = new SubtitleService(_subtitleContextFactory);
        using (var sub = TempSubtitle.Create())
        {
            instance.AddSubtitlesFromFile(sub.Filename);
        }
        Assert.Single(instance.AvailableSubtitles);
    }

    [Fact]
    public void CanCallClearSubtitles()
    {
        var instance = new SubtitleService(_subtitleContextFactory);
        using (var sub = TempSubtitle.Create())
        {
            instance.AddSubtitlesFromFile(sub.Filename);
        }
        instance.ClearSubtitles();
        Assert.Empty(instance.AvailableSubtitles);
    }

    [Fact]
    public void CanCallActivateSubtitle()
    {
        var instance = new SubtitleService(_subtitleContextFactory);
        using (var sub = TempSubtitle.Create())
        {
            instance.AddSubtitlesFromFile(sub.Filename);
        }
        var info = instance.AvailableSubtitles.First();
        instance.ActivateSubtitle(info);
        Assert.True(instance.IsSubtitleActive);
    }

    [Fact]
    public void CanCallGetSubtitle()
    {
        var time = new TimeSpan(0, 0, 8);
        var instance = new SubtitleService(new SubtitleContextFactory());
        using (var sub = TempSubtitle.Create())
        {
            instance.AddSubtitlesFromFile(sub.Filename);
        }
        var info = instance.AvailableSubtitles.First();
        instance.ActivateSubtitle(info);
        var subt = instance.GetSubtitle(time);
        Assert.NotNull(subt);
        Assert.True(subt.ToString().Length > 5);
    }

    [Fact]
    public void CanGetIsSubtitleActive()
    {
        Assert.IsType<bool>(_testClass.IsSubtitleActive);
    }
}
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
