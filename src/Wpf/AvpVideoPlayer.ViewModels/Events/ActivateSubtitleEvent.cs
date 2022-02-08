using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class ActivateSubtitleEvent : EventBase<SubtitleInfo>
{
    public ActivateSubtitleEvent(SubtitleInfo subtitleInfo) : base(subtitleInfo)
    {

    }
}
