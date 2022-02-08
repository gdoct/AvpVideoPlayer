using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class ActivateSubtitleEvent : EventBase<SubtitleInfo>
{
    public ActivateSubtitleEvent(SubtitleInfo subtitleInfo) : base(subtitleInfo)
    {

    }
}
