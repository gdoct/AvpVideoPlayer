using AvpVideoPlayer.Uwp.Utility;
using AvpVideoPlayer.Uwp.ViewModels;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AvpVideoPlayer.Uwp.Views
{
    public sealed partial class VideoPlayer : UserControl
    {
        public VideoPlayer()
        {
            this.InitializeComponent();
            // to get access to the media element from the viewmodel (we need to be able to call methods on it such as start and stop)
            // we wrap it in an IVIdeoPlayerView and register it through the IViewRegistrationService. The "Instance" Singleton is the
            // same instance as the singleton registered in the DI framework.
            ViewRegistrationService.Instance.Register(Api.ViewResources.MediaElement, new VideoPlayerView(MediaPlayer));
        }
    }
}
