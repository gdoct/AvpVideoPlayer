namespace AvpVideoPlayer.MetaData.Tests;
using AvpVideoPlayer.MetaData;
using System.IO;
using System.Reflection;
using Xunit;

public class FileMetaDataComparerTests
{
    private readonly FileMetaDataComparer _testClass;

    public FileMetaDataComparerTests()
    {
        _testClass = new FileMetaDataComparer();
    }

    [Fact]
    public void CanCallEquals()
    {
        // Arrange
        var fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
        var fi2 = new FileInfo("c:\\test\\test.txt");
        var left = FileMetaData.Create(fi);
        var right = FileMetaData.Create(fi);
        var right2 = FileMetaData.Create(fi2);

        // Act
        var result = _testClass.Equals(left, right);
        var result2 = _testClass.Equals(left, right2);

        // Assert
        Assert.True(result);
        Assert.False(result2);
    }

    [Fact]
    public void CanCallGetHashCode()
    {
        // Arrange
        var fi = new FileInfo(Assembly.GetExecutingAssembly().Location);
        var fi2 = new FileInfo("c:\\test\\test.txt");
        var left = FileMetaData.Create(fi);
        var right = FileMetaData.Create(fi);
        var right2 = FileMetaData.Create(fi2);

        // Act
        var result1 = left.GetHashCode();
        var result2 = right.GetHashCode();
        var result3 = right2.GetHashCode();

        // Assert
        Assert.NotEqual(result1, result2);
        Assert.NotEqual(result1, result3);
    }
}
