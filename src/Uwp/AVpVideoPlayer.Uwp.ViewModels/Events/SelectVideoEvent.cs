using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class SelectVideoEvent : EventBase<string>
{
    public SelectVideoEvent(string eventdata) : base(eventdata)
    {
    }
}
