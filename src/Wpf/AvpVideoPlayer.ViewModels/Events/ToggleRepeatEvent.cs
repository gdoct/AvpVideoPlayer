using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class ToggleRepeatEvent : EventBase<bool>
{
    public ToggleRepeatEvent(bool state) : base(state)
    {

    }

    public bool IsHandled { get; set; }
    public SubtitleInfo? ActiveSubtitle { get; set; }
}
