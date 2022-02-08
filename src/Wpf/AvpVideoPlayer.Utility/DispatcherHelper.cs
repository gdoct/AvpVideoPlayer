using System.Windows;

namespace AvpVideoPlayer.Utility;

public static class DispatcherHelper
{
    public static void Invoke(Action action)
    {
        var dispatcher = Application.Current?.Dispatcher;
        if (dispatcher != null)
        {
            dispatcher.Invoke(action);
        }
        else
            action();
    }
}
