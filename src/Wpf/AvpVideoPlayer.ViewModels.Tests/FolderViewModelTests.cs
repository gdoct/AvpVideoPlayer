namespace AvpVideoPlayer.ViewModels.Tests;

using AvpVideoPlayer.ViewModels.IO;
using System.IO;
using Xunit;

public class FolderViewModelTests
{
    private readonly string _path;
    private readonly string _name;

    public FolderViewModelTests()
    {
        _path = "TestValue2017163036";
        _name = "TestValue998189580";
    }

    [Fact]
    public void CanConstruct()
    {
        var instance = new FolderViewModel(_path);
        Assert.NotNull(instance);
        instance = new FolderViewModel(_path, _name);
        Assert.NotNull(instance);
    }

    [Theory]
    [InlineData(@"C:\testc.txt")]
    [InlineData(@"\\server\folder\testc.txt")]
    [InlineData(@"test1c.txt")]
    public void PathShouldBecomeFuileInfo(string file)
    {
        var fi = new FileInfo(file);
        var instance = new FolderViewModel(file);
        Assert.Equal(fi.FullName, instance.FileInfo?.FullName);
    }


}
