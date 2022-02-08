namespace AvpVideoPlayer.ViewModels.Tests;

using AvpVideoPlayer.Api;
using AvpVideoPlayer.Video.Snapshot;
using Moq;
using System;
using Xunit;


public class VideoPlayerViewModelTests
{
    private readonly VideoPlayerViewModel _testClass;

    public VideoPlayerViewModelTests()
    {
        var eh = new Mock<IEventHub>();
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        _testClass = new VideoPlayerViewModel(eh.Object, new PlayerControlsViewModel(eh.Object,
                                                                                     Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<ISnapshotService>()), Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<IUserConfiguration>());
    }

    [Fact]
    public void CanConstruct()
    {
        var eh = new Mock<IEventHub>();
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        var instance = new VideoPlayerViewModel(eh.Object,
                                                new PlayerControlsViewModel(eh.Object, Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<ISnapshotService>()),
                                                Mock.Of<IViewRegistrationService>(),
                                                Mock.Of<IUserConfiguration>());
        Assert.NotNull(instance);
    }

    [Fact]
    public void CanSetAndGetSubtitleFontSize()
    {
        _testClass.CheckProperty(x => x.SubtitleFontSize, 1617374552.31, 1508889747.42);
    }

}
