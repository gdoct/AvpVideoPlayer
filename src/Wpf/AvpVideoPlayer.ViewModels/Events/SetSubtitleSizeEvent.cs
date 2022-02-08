using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class SetSubtitleSizeEvent : EventBase<int>
{
    public SetSubtitleSizeEvent(int eventdata) : base(eventdata)
    {
    }
}
