namespace AvpVideoPlayer.ViewModels.Tests;

using AvpVideoPlayer.Api;
using Moq;
using Xunit;
using System;
using AvpVideoPlayer.Video.Snapshot;
using AvpVideoPlayer.ViewModels.Controls;

public class PlayerControlsViewModelTests
{
    private readonly PlayerControlsViewModel _testClass;

    public PlayerControlsViewModelTests()
    {
        var eh = new Mock<IEventHub>();
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        _testClass = new PlayerControlsViewModel(eh.Object, Mock.Of <IViewRegistrationService>(),
                                           Mock.Of<ISnapshotService>(), Mock.Of<IDispatcherHelper>());
    }

    [Fact]
    public void CanConstruct()
    {
        var eh = new Mock<IEventHub>();
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        var instance = new PlayerControlsViewModel(eh.Object, Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<ISnapshotService>(), Mock.Of<IDispatcherHelper>());
        Assert.NotNull(instance);
    }

    [Fact]
    public void CanSetAndGetIsPlaying()
    {
        _testClass.CheckProperty(x => x.IsPlaying);
    }

    [Fact]
    public void CanSetAndGetIsSubtitlesVisible()
    {
        _testClass.CheckProperty(x => x.IsSubtitlesVisible);
    }
}
