using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class SelectedFileChangedEvent : EventBase<FileViewModel>
{
    public SelectedFileChangedEvent(FileViewModel path) : base(path)
    {

    }
}
