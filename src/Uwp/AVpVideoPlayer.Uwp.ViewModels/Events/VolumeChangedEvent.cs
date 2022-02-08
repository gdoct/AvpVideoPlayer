using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class VolumeChangedEvent : EventBase<double>
{
    public VolumeChangedEvent(double eventdata) : base(eventdata)
    {
    }
}
