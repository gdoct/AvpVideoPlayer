using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class VolumeChangeRequestEvent : EventBase<double>
{
    public VolumeChangeRequestEvent(double eventdata) : base(eventdata)
    {
    }
}
