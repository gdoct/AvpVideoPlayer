using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class PlayPositionChangedEvent : EventBase<double>
{
    public PlayPositionChangedEvent(double eventdata) : base(eventdata)
    {
    }
}
