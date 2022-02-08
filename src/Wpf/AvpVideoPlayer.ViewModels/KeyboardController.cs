using AvpVideoPlayer.Api;
using AvpVideoPlayer.ViewModels.Events;
using System.Windows.Input;

namespace AvpVideoPlayer.ViewModels;

public class KeyboardController : IKeyboardController
{
    private readonly IEventHub _eventHub;

    public KeyboardController(IEventHub eventHub)
    {
        _eventHub = eventHub;
    }

    public bool ProcessKeypress(Key key)
    {
        bool handled = true;
        switch (key)
        {
            case Key.Escape: _eventHub.Publish(new FullScreenEvent(false)); break;
            case Key.Space: _eventHub.Publish(new PlayStateChangeRequestEvent(PlayStates.Toggle)); break;
            case Key.Left: _eventHub.Publish(new PlaylistMoveEvent(PlayListMoveTypes.Back)); break;
            case Key.Right: _eventHub.Publish(new PlaylistMoveEvent(PlayListMoveTypes.Forward)); break;
            case Key.Delete: _eventHub.Publish(new DeleteCurrentVideoEvent()); break;
            default: handled = false; break;
        }
        return handled;

    }
}
