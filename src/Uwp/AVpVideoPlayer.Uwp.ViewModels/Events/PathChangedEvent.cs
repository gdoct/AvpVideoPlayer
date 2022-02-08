using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class PathChangedEvent : EventBase<string>
{
    public PathChangedEvent(string eventdata) : base(eventdata)
    {
    }
}
