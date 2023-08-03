using System.Windows.Input;

namespace AvpVideoPlayer.ViewModels.Views;

public interface IKeyboardController
{
    bool ProcessKeypress(Key key);
}
