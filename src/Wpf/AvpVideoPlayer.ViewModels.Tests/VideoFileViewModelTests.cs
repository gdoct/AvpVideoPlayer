namespace AvpVideoPlayer.ViewModels.Tests;

using AvpVideoPlayer.ViewModels.IO;
using System.IO;
using Xunit;

public class VideoFileViewModelTests
{
    private readonly FileInfo _file;

    public VideoFileViewModelTests()
    {
        _file = new FileInfo("TestValue1249605200");
    }

    [Fact]
    public void CanConstruct()
    {
        var instance = new VideoFileViewModel(_file);
        Assert.NotNull(instance);
    }
}
