using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class PlayDurationChangedEvent : EventBase<double>
{
    public PlayDurationChangedEvent(double eventdata) : base(eventdata)
    {
    }
}
