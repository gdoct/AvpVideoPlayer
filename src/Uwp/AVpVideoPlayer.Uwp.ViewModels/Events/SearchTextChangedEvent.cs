using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class SearchTextChangedEvent : EventBase<string>
{
    public SearchTextChangedEvent(string eventdata) : base(eventdata)
    {
    }
}
