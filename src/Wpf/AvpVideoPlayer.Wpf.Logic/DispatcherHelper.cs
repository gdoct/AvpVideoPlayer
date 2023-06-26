using AvpVideoPlayer.Api;
using System.Windows;

namespace AvpVideoPlayer.Wpf.Logic;

public class DispatcherHelper : IDispatcherHelper
{
    public void Invoke(Action action)
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
