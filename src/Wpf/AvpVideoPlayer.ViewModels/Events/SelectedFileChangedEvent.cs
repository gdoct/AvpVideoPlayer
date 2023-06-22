using AvpVideoPlayer.Api;
using AvpVideoPlayer.ViewModels.IO;

namespace AvpVideoPlayer.ViewModels.Events;

public class SelectedFileChangedEvent : EventBase<FileViewModel>
{
    public SelectedFileChangedEvent(FileViewModel path) : base(path)
    {

    }
}
