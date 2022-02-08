using AvpVideoPlayer.Uwp.Api;

namespace AvpVideoPlayer.Uwp.ViewModels.Events;

public class LoadSubtitlesEvent : EventBase<string>
{
    public LoadSubtitlesEvent(string eventdata) : base(eventdata)
    {
    }
    public string Filename => Data;
}
