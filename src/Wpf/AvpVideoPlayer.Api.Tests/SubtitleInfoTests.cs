namespace AvpVideoPlayer.Api.Tests;

using Xunit;

public class SubtitleInfoTests
{
    private readonly SubtitleInfo _testClass;

    public SubtitleInfoTests()
    {
        _testClass = new SubtitleInfo();
    }

    [Fact]
    public void CanSetAndGetSubtitleName()
    {
        var testValue = "TestValue359247789";
        _testClass.SubtitleName = testValue;
        Assert.Equal(testValue, _testClass.SubtitleName);
    }

    [Fact]
    public void CanSetAndGetStreamInfo()
    {
        var testValue = "TestValue684287269";
        _testClass.StreamInfo = testValue;
        Assert.Equal(testValue, _testClass.StreamInfo);
    }

    [Fact]
    public void CanSetAndGetIndex()
    {
        var testValue = 1798787827;
        _testClass.Index = testValue;
        Assert.Equal(testValue, _testClass.Index);
    }

    [Fact]
    public void CanSetAndGetFilename()
    {
        var testValue = "TestValue1600362892";
        _testClass.VideoFilename = testValue;
        Assert.Equal(testValue, _testClass.VideoFilename);
    }
}
