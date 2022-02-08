using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class PlaylistMoveEvent : EventBase<PlayListMoveTypes>
{
    public PlaylistMoveEvent(PlayListMoveTypes eventdata) : base(eventdata)
    {
    }
}
