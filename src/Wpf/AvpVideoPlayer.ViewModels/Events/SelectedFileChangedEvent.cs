using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class SelectedFileChangedEvent : EventBase<FileViewModel>
{
    public SelectedFileChangedEvent(FileViewModel path) : base(path)
    {

    }
}
