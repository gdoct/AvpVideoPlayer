namespace AvpVideoPlayer.ViewModels.Tests;

using Xunit;

public class BaseViewModelTests
{
    public BaseViewModelTests()
    {
    }

    [Fact]
    public void CanConstruct()
    {
        var instance = new BaseViewModel();
        Assert.NotNull(instance);
    }
}
