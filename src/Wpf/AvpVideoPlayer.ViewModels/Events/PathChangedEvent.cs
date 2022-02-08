using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class PathChangedEvent : EventBase<string>
{
    public PathChangedEvent(string eventdata) : base(eventdata)
    {
    }
}
