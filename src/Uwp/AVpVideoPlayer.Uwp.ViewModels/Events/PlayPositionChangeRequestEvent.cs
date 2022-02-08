using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class PlayPositionChangeRequestEvent : EventBase<double>
{
    public PlayPositionChangeRequestEvent(double eventdata) : base(eventdata)
    {
    }
}
