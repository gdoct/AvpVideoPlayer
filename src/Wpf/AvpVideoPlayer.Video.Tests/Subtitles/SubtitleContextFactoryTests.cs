namespace AvpVideoPlayer.Subtitles.Tests;

using AvpVideoPlayer.Video.Subtitles;
using System.IO;
using Xunit;

public class SubtitleContextFactoryTests
{
    private readonly SubtitleContextFactory _subtitleContextFactory;

    public SubtitleContextFactoryTests()
    {
        _subtitleContextFactory = new SubtitleContextFactory();
    }

    [Fact]
    public void CanCallFromFile()
    {
        using var sub = TempSubtitle.Create();
        var result = _subtitleContextFactory.FromFile(sub.Filename);
        Assert.NotNull(result);
        Assert.True(result.GetSubtitleForTime(new(0, 0, 7)) != null);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("ds dffsds ")]
    [InlineData("#&$!")]
    [InlineData("C:\\Windows\\System128\\Data")]
    public void CannotCallFromFileWithInvalidFilename(string value)
    {
        Assert.Throws<FileNotFoundException>(() => _subtitleContextFactory.FromFile(value));
    }

    //[Fact]
    //public void CanCallFromVideoFile()
    //{
    //    var filename = "TestValue841592382";
    //    var result = _subtitleContextFactory.FromVideoFile(filename);
    //    throw new NotImplementedException("Create or modify test");
    //}

    [Fact]
    public void CanCallEmpty()
    {
        var result = _subtitleContextFactory.Empty();
        Assert.IsType<EmptySubtitleContext>(result);
    }
}
