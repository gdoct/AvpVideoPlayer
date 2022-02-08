using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class PlayDurationChangedEvent : EventBase<double>
{
    public PlayDurationChangedEvent(double eventdata) : base(eventdata)
    {
    }
}
