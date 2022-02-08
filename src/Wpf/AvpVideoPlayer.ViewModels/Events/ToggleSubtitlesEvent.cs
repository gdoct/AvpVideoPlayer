using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class ToggleSubtitlesEvent : EventBase<bool>
{
    public ToggleSubtitlesEvent(bool state) : base(state)
    {

    }

    public bool IsHandled { get; set; }
    public SubtitleInfo? ActiveSubtitle { get; set; }
}
