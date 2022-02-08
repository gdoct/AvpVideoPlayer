using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.ViewModels.Events;

public class FullScreenEvent : EventBase<bool>
{
    public FullScreenEvent(bool isFullscreen) : base(isFullscreen)
    {
    }
    public bool IsFullScreen => Data;
}
