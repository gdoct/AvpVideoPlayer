using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class SetSubtitleSizeEvent : EventBase<int>
{
    public SetSubtitleSizeEvent(int eventdata) : base(eventdata)
    {
    }
}
