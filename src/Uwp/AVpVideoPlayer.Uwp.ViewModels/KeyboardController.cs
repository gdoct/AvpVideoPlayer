using AvpVideoPlayer.Uwp.Api;
using AvpVideoPlayer.Uwp.ViewModels.Events;
using System.Windows.Input;
using Windows.System;

namespace AvpVideoPlayer.Uwp.ViewModels;

public class KeyboardController : IKeyboardController
{
    private readonly IEventHub _eventHub;

    public KeyboardController(IEventHub eventHub)
    {
        _eventHub = eventHub;
    }

    public bool ProcessKeypress(VirtualKey key)
    {
        bool handled = true;
        switch (key)
        {
            case VirtualKey.Escape: _eventHub.Publish(new FullScreenEvent(false)); break;
            case VirtualKey.Space: _eventHub.Publish(new PlayStateChangeRequestEvent(PlayStates.Toggle)); break;
            case VirtualKey.Left: _eventHub.Publish(new PlaylistMoveEvent(PlayListMoveTypes.Back)); break;
            case VirtualKey.Right: _eventHub.Publish(new PlaylistMoveEvent(PlayListMoveTypes.Forward)); break;
            case VirtualKey.Delete: _eventHub.Publish(new DeleteCurrentVideoEvent()); break;
            default: handled = false; break;
        }
        return handled;

    }
}
