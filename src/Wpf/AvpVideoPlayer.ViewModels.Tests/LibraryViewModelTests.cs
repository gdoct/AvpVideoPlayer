namespace AvpVideoPlayer.ViewModels.Tests;
using AvpVideoPlayer.Api;
using AvpVideoPlayer.MetaData;
using AvpVideoPlayer.ViewModels;
using AvpVideoPlayer.ViewModels.Controls;
using AvpVideoPlayer.ViewModels.IO;
using Moq;
using System;
using Xunit;

public class LibraryViewModelTests
{
    private readonly IUserConfiguration _userSettingsService;

    public LibraryViewModelTests()
    {
        _userSettingsService = Mock.Of<IUserConfiguration>();
    }

    [Fact]
    public void CanConstruct()
    {
        var eh = new Mock<IEventHub>();
        var md = new Mock<IMetaDataService>();
        var dh = Mock.Of<IDispatcherHelper>();
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        var instance = new LibraryViewModel(_userSettingsService, eh.Object, Mock.Of<IDialogService>(), dh, new SearchBoxViewModel(eh.Object), new FolderDropDownViewModel(dh), 
            new FileListViewModel(eh.Object, md.Object, new M3UService(), dh));
        Assert.NotNull(instance);
    }

    [Fact]
    public void ShouldProcessEvent()
    {
        try
        {
            var eh = new Mock<IEventHub>();
            var dh = Mock.Of<IDispatcherHelper>();
            var md = new Mock<IMetaDataService>();
            eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
            var instance = new LibraryViewModel(_userSettingsService, eh.Object, Mock.Of<IDialogService>(), dh, new SearchBoxViewModel(eh.Object), new FolderDropDownViewModel(dh), new FileListViewModel(eh.Object, md.Object, new M3UService(), dh));
            instance.OnSelectFile(new Events.SelectedFileChangedEvent(new FileViewModel(FileTypes.Folder)));
        } 
        catch (Exception ex)
        {
            Assert.Fail(ex.Message);
            throw;
        }
    }
}
