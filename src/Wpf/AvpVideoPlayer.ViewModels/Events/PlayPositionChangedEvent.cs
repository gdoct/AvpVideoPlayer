using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class PlayPositionChangedEvent : EventBase<double>
{
    public PlayPositionChangedEvent(double eventdata) : base(eventdata)
    {
    }
}
