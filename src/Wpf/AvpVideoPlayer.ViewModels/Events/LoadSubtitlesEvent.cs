using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class LoadSubtitlesEvent : EventBase<string>
{
    public LoadSubtitlesEvent(string eventdata) : base(eventdata)
    {
    }
    public string Filename => Data;
}
