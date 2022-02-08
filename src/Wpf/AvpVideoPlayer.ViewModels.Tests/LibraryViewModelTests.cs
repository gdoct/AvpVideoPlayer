namespace AvpVideoPlayer.ViewModels.Tests;
using AvpVideoPlayer.Api;
using AvpVideoPlayer.ViewModels;
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
        eh.Setup(eh => eh.Events).Returns(Mock.Of<IObservable<EventBase>>());
        var instance = new LibraryViewModel(_userSettingsService, eh.Object, Mock.Of<IDialogService>(), new SearchBoxViewModel(eh.Object), new FolderDropDownViewModel(), new FileListViewModel(eh.Object));
        Assert.NotNull(instance);
    }
}
