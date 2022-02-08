using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class VolumeChangeRequestEvent : EventBase<double>
{
    public VolumeChangeRequestEvent(double eventdata) : base(eventdata)
    {
    }
}
