using Windows.System;

namespace AvpVideoPlayer.Uwp.Api;

public interface IKeyboardController
{
    bool ProcessKeypress(VirtualKey key);
}
