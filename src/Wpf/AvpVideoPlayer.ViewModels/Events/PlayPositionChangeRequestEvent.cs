using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class PlayPositionChangeRequestEvent : EventBase<double>
{
    public PlayPositionChangeRequestEvent(double eventdata) : base(eventdata)
    {
    }
}
