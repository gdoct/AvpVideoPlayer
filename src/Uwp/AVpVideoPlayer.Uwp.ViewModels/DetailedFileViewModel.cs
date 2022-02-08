using AvpVideoPlayer.Uwp.Api;
using AvpVideoPlayer.Uwp.Utility;
using System.Windows.Input;

namespace AvpVideoPlayer.Uwp.ViewModels;

public class DetailedFileViewModel : BaseViewModel
{
    private readonly IEventHub _eventHub;

    public ICommand PlayVideoCommand { get; }

    public DetailedFileViewModel(IEventHub eventHub)
    {
        _eventHub = eventHub;
        PlayVideoCommand = new RelayCommand(PlayVideo);
    }

    private FileViewModel _file = null;
    private string filename = string.Empty;

    public FileViewModel FiletypeViewModel
    {
        get => _file;
        set
        {
            _file = value;
            UpdateProperties();
        }
    }

    private void UpdateProperties()
    {
        Filename = _file?.Name ?? "<empty>";
    }

    public string Filename { get => filename; set => SetProperty(ref filename, value); }

    private void PlayVideo()
    {
        if (this.FiletypeViewModel?.FileInfo == null) return;
        _eventHub.Publish(new ViewModels.Events.SelectVideoEvent
            (this.FiletypeViewModel.FileInfo.FullName));
    }
}
