using AvpVideoPlayer.Uwp.Api;
using System.Collections.Generic;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class SubtitlesLoadedEvent : EventBase<IList<SubtitleInfo>>
{
    public SubtitlesLoadedEvent(IList<SubtitleInfo> data) : base(data)
    {

    }
}
