using System.Windows.Input;

namespace AvpVideoPlayer.Api;

public interface IKeyboardController
{
    bool ProcessKeypress(Key key);
}
