using AvpVideoPlayer.Utility;
using AvpVideoPlayer.ViewModels;
using System.Windows.Controls;

namespace AvpVideoPlayer.Wpf.Views;

/// <summary>
/// Interaction logic for VideoPlayer.xaml
/// </summary>
public partial class VideoPlayer : UserControl
{
    public VideoPlayer()
    {
        InitializeComponent();
        // to get access to the media element from the viewmodel (we need to be able to call methods on it such as start and stop)
        // we wrap it in an IVIdeoPlayerView and register it through the IViewRegistrationService. The "Instance" Singleton is the
        // same instance as the singleton registered in the DI framework.
        ViewRegistrationService.Instance.Register(Api.ViewResources.MediaElement, new VideoPlayerView(MediaPlayer));
    }
}