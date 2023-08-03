namespace AvpVideoPlayer.ViewModels.Tests;

using AvpVideoPlayer.Api;
using AvpVideoPlayer.MetaData;
using AvpVideoPlayer.Video.Snapshot;
using AvpVideoPlayer.ViewModels.Controls;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;


public class VideoPlayerViewModelTests
{
    private readonly VideoPlayerViewModel _testClass;

    public VideoPlayerViewModelTests()
    {
        var dh = Mock.Of<IDispatcherHelper>();
        var itd = Mock.Of<IIdleTimeDetector>();
        var eh = new Mock<IEventHub>();
        var ts = new Mock<ITaggingService>();
        ts.Setup(ts => ts.GetTags()).Returns(new List<string>());
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        _testClass = new VideoPlayerViewModel(eh.Object,
                                              new PlayerControlsViewModel(eh.Object, Mock.Of<IViewRegistrationService>(), Mock.Of<ISnapshotService>(), dh),
                                              Mock.Of<IViewRegistrationService>(),
                                              Mock.Of<IUserConfiguration>(),
                                              Mock.Of<IMetaDataService>(),
                                              ts.Object,
                                              dh,
                                              itd);
    }

    [Fact]
    public void CanConstruct()
    {
        var dh = Mock.Of<IDispatcherHelper>();
        var itd = Mock.Of<IIdleTimeDetector>();
        var eh = new Mock<IEventHub>();
        var ts = new Mock<ITaggingService>();
        ts.Setup(ts => ts.GetTags()).Returns(new List<string>());
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        var instance = new VideoPlayerViewModel(eh.Object,
                                                new PlayerControlsViewModel(eh.Object, Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<ISnapshotService>(), dh),
                                                Mock.Of<IViewRegistrationService>(),
                                                Mock.Of<IUserConfiguration>(),
                                           Mock.Of<IMetaDataService>(),
                                           ts.Object,
                                              dh,
                                              itd);
        Assert.NotNull(instance);
    }

    [Fact]
    public void CanSetAndGetSubtitleFontSize()
    {
        _testClass.CheckProperty(x => x.SubtitleFontSize, 1617374552.31, 1508889747.42);
    }

}
