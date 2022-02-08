namespace AvpVideoPlayer.ViewModels.Tests;

using AvpVideoPlayer.Api;
using Moq;
using System;
using Xunit;

public class SearchBoxViewModelTests
{
    public SearchBoxViewModelTests()
    {
    }

    [Fact]
    public void CanConstruct()
    {
        var eh = new Mock<IEventHub>();
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        var instance = new SearchBoxViewModel(eh.Object);
        Assert.NotNull(instance);
    }
}
