using Xunit;

namespace AvpVideoPlayer.Api.Tests;


public class ConstantsTests
{

    public ConstantsTests()
    {
    }

    [Fact]
    public void ConstTests()
    {
        Assert.NotNull(Constants.SettingNames.LastPathSetting);
        Assert.NotNull(Constants.SettingNames.Keyroot);
        Assert.NotNull(Constants.FfMpegExe);
    }
}
