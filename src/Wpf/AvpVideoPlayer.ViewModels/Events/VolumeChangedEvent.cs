using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class VolumeChangedEvent : EventBase<double>
{
    public VolumeChangedEvent(double eventdata) : base(eventdata)
    {
    }
}
