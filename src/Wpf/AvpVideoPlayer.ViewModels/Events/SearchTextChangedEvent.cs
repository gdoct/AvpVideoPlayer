using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class SearchTextChangedEvent : EventBase<string>
{
    public SearchTextChangedEvent(string eventdata) : base(eventdata)
    {
    }
}
