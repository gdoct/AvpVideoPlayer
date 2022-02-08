using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class PlayStateChangeRequestEvent : EventBase<PlayStates>
{
    public PlayStateChangeRequestEvent(PlayStates eventdata) : base(eventdata)
    {

    }
}
