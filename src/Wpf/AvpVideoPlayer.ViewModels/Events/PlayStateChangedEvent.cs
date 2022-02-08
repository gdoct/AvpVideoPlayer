using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class PlayStateChangedEvent : EventBase<PlayStates>
{
    public PlayStateChangedEvent(PlayStates eventdata) : base(eventdata)
    {
    }

    public string? Filename { get; set; }
}
