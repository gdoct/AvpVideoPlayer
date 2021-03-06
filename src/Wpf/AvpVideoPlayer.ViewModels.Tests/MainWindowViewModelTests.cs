namespace AvpVideoPlayer.ViewModels.Tests;

using AvpVideoPlayer.Api;
using AvpVideoPlayer.Video.Snapshot;
using Moq;
using System;
using System.Windows;
using Xunit;

public class MainWindowViewModelTests
{
    private readonly MainWindowViewModel _testClass;

    public MainWindowViewModelTests()
    {
        var eh = new Mock<IEventHub>();
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        var lv = new LibraryViewModel(Mock.Of<IUserConfiguration>(),
                                      eh.Object,
                                      Mock.Of<IDialogService>(),
                                      new SearchBoxViewModel(eh.Object),
                                      new FolderDropDownViewModel(),
                                      new FileListViewModel(eh.Object));
        var vpv = new VideoPlayerViewModel(eh.Object,
                                           new PlayerControlsViewModel(eh.Object, Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<ISnapshotService>()),
                                           Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<IUserConfiguration>());
        _testClass = new MainWindowViewModel(eh.Object, lv, vpv);
    }

    [Fact]
    public void CanConstruct()
    {
        var eh = new Mock<IEventHub>();
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        var lv = new LibraryViewModel(Mock.Of<IUserConfiguration>(),
                                      eh.Object,
                                      Mock.Of<IDialogService>(),
                                      new SearchBoxViewModel(eh.Object),
                                      new FolderDropDownViewModel(),
                                      new FileListViewModel(eh.Object));
        var vpv = new VideoPlayerViewModel(eh.Object,
                                           new PlayerControlsViewModel(eh.Object, Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<ISnapshotService>()),
                                           Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<IUserConfiguration>());
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
