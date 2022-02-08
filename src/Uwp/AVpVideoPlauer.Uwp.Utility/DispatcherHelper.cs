using System;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace AvpVideoPlayer.Uwp.Utility;

public static class DispatcherHelper
{
    [Deprecated("Use the dispatcher of the view instead", DeprecationType.Deprecate, 1)]
    public static void Invoke(Action action)
    {
        action();
    }
}
