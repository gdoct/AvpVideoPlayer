using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class PlayPositionChangeRequestEvent : EventBase<(double, bool)>
{   
    public PlayPositionChangeRequestEvent(double eventdata, bool relative = false) : base((eventdata, relative))
    {
    }
}
