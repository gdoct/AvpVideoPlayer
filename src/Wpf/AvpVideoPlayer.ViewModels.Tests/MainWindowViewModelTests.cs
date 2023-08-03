namespace AvpVideoPlayer.ViewModels.Tests;

using AvpVideoPlayer.Api;
using AvpVideoPlayer.MetaData;
using AvpVideoPlayer.Video.Snapshot;
using AvpVideoPlayer.ViewModels.Controls;
using AvpVideoPlayer.ViewModels.Views;
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
        var dh = Mock.Of<IDispatcherHelper>();
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        var lv = new LibraryViewModel(Mock.Of<IUserConfiguration>(),
                                      eh.Object,
                                      Mock.Of<IDialogService>(),
                                      dh,
                                      new SearchBoxViewModel(eh.Object),
                                      new FolderDropDownViewModel(dh),
                                      new FileListViewModel(eh.Object, md.Object, new M3UService(), dh));
        var vpv = new VideoPlayerViewModel(eh.Object,
                                           new PlayerControlsViewModel(eh.Object, Mock.Of<IViewRegistrationService>(), Mock.Of<ISnapshotService>(), dh),
                                           Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<IUserConfiguration>(),
                                           Mock.Of<IMetaDataService>(),
                                           ts.Object,
                                           dh,
                                           Mock.Of<IIdleTimeDetector>());
        _testClass = new MainWindowViewModel(eh.Object, lv, vpv);
    }

    [Fact]
    public void CanConstruct()
    {
        var md = new Mock<IMetaDataService>();
        var ts = new Mock<ITaggingService>();
        ts.Setup(ts => ts.GetTags()).Returns(new List<string>());
        var dh = Mock.Of<IDispatcherHelper>();
        var eh = new Mock<IEventHub>();
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        var lv = new LibraryViewModel(Mock.Of<IUserConfiguration>(),
                                      eh.Object,
                                      Mock.Of<IDialogService>(),
                                      dh,
                                      new SearchBoxViewModel(eh.Object),
                                      new FolderDropDownViewModel(dh),
                                      new FileListViewModel(eh.Object, md.Object, new M3UService(), dh));
        var vpv = new VideoPlayerViewModel(eh.Object,
                                           new PlayerControlsViewModel(eh.Object, Mock.Of<IViewRegistrationService>(), Mock.Of<ISnapshotService>(), dh),
                                           Mock.Of<IViewRegistrationService>(),
                                           Mock.Of<IUserConfiguration>(),
                                           Mock.Of<IMetaDataService>(),
                                           ts.Object, 
                                           dh,
                                           Mock.Of<IIdleTimeDetector>());
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

    [Theory]
    [InlineData(640, 399, 9)]
    [InlineData(1024, 768, 19)]
    [InlineData(1920, 1080, 48)]
    [InlineData(3840, 2160, 64)]
    public void SetsCorrectFontSize(int width, int height, int expected)
    {
        var sz = new Size(width, height);
        var result = MainWindowViewModel.ConvertWindowToFontSize(sz);
        Assert.Equal(expected, result);
    }
}
