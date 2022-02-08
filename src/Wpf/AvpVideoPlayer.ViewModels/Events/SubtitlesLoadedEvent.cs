using AvpVideoPlayer.Api;
using System.Collections.Generic;

namespace AvpVideoPlayer.ViewModels.Events;

public class SubtitlesLoadedEvent : EventBase<IList<SubtitleInfo>>
{
    public SubtitlesLoadedEvent(IList<SubtitleInfo> data) : base(data)
    {

    }
}
