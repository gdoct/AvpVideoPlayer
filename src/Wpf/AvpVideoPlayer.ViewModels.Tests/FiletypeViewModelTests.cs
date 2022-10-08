namespace AvpVideoPlayer.ViewModels.Tests;
using AvpVideoPlayer.Api;
using Xunit;

public class FiletypeViewModelTests
{
    private readonly FileViewModel _testClass;
    private readonly FileTypes _filetype;
    const string TESTPATH = @"c:\test\test.txt";

    public FiletypeViewModelTests()
    {
        _filetype = FileTypes.Subtitles;
        _testClass = new FileViewModel(_filetype) { Path = TESTPATH };
    }

    [Fact]
    public void CanConstruct()
    {
        var instance = new FileViewModel(_filetype) { Path = TESTPATH };
        Assert.Equal(instance.Name, _testClass.Name);
        Assert.Equal(instance.Path, _testClass.Path);
        Assert.Equal(instance.LastWriteTime, _testClass.LastWriteTime);
        Assert.Equal(instance.Size, _testClass.Size);
        Assert.NotNull(instance);
    }

    [Fact]
    public void CanConstructNull()
    {
        var instance = new FileViewModel(FileTypes.Video) { Path = null };
        Assert.Equal(System.DateTime.Today, instance.LastWriteTime);
        Assert.Null(instance.FileInfo);
        Assert.Equal(0, instance.Size);
    }

    [Fact]
    public void DirectoryHas0Bytes()
    {
        var instance = new FileViewModel(FileTypes.Folder) { Path = null };
        Assert.Equal(0, instance.Size);
    }

    [Fact]
    public void FileTypeIsInitializedCorrectly()
    {
        Assert.Equal(_filetype, _testClass.FileType);
    }


    [Fact]
    public void CanGetSize()
    {
        Assert.IsType<long>(_testClass.Size);
    }
}
