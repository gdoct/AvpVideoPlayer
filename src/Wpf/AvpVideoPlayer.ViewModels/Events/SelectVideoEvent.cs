using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class SelectVideoEvent : EventBase<string>
{
    public SelectVideoEvent(string eventdata) : base(eventdata)
    {
    }
}
