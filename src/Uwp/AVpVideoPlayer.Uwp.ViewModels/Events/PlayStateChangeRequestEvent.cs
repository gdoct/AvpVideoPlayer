using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class PlayStateChangeRequestEvent : EventBase<PlayStates>
{
    public PlayStateChangeRequestEvent(PlayStates eventdata) : base(eventdata)
    {

    }
}
