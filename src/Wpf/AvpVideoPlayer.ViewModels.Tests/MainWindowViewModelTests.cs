namespace AvpVideoPlayer.ViewModels.Tests;

using AvpVideoPlayer.Api;
using AvpVideoPlayer.MetaData;
using AvpVideoPlayer.Video.Snapshot;
using Moq;
using System;
using System.Collections.Generic;
using System.Windows;
using Xunit;

public class MainWindowViewModelTests
{
    private readonly MainWindowViewModel _testClass;

    public MainWindowViewModelTests()
    {
        var md = new Mock<IMetaDataService>();
        var ts = new Mock<ITaggingService>();
        ts.Setup(ts => ts.GetTags()).Returns(new List<string>());
        var eh = new Mock<IEventHub>();
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        var lv = new LibraryViewModel(Mock.Of<IUserConfiguration>(),
                                      eh.Object,
                                      Mock.Of<IDialogService>(),
                                      new SearchBoxViewModel(eh.Object),
                                      new FolderDropDownViewModel(),
                                      new FileListViewModel(eh.Object, md.Object, new M3uService()));
        var vpv = new VideoPlayerViewModel(eh.Object,
                                           new PlayerControlsViewModel(eh.Object, Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<ISnapshotService>()),
                                           Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<IUserConfiguration>(),
                                           Mock.Of<IMetaDataService>(),
                                           ts.Object);
        _testClass = new MainWindowViewModel(eh.Object, lv, vpv);
    }

    [Fact]
    public void CanConstruct()
    {
        var md = new Mock<IMetaDataService>();
        var ts = new Mock<ITaggingService>();
        ts.Setup(ts => ts.GetTags()).Returns(new List<string>());
        var eh = new Mock<IEventHub>();
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        var lv = new LibraryViewModel(Mock.Of<IUserConfiguration>(),
                                      eh.Object,
                                      Mock.Of<IDialogService>(),
                                      new SearchBoxViewModel(eh.Object),
                                      new FolderDropDownViewModel(),
                                      new FileListViewModel(eh.Object, md.Object, new M3uService()));
        var vpv = new VideoPlayerViewModel(eh.Object,
                                           new PlayerControlsViewModel(eh.Object, Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<ISnapshotService>()),
                                           Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<IUserConfiguration>(),
                                           Mock.Of<IMetaDataService>(),
                                           ts.Object);
        var instance = new MainWindowViewModel(eh.Object, lv, vpv);
        Assert.NotNull(instance);
    }

    [Fact]
    public void CanSetAndGetTitle()
    {
        _testClass.CheckProperty(x => x.Title);
    }

    [Fact]
    public void CanSetAndGetWindowState()
    {
        _testClass.CheckProperty(x => x.WindowState, WindowState.Minimized, WindowState.Maximized);
    }
}
