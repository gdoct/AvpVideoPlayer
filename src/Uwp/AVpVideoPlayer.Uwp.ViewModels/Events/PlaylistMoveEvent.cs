using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class PlaylistMoveEvent : EventBase<PlayListMoveTypes>
{
    public PlaylistMoveEvent(PlayListMoveTypes eventdata) : base(eventdata)
    {
    }
}
