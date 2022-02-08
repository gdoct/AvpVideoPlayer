using AvpVideoPlayer.Uwp.Api;
using AvpVideoPlayer.Uwp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AvpVideoPlayer.Uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var eh = DependencyInjection.Services.GetService<IEventHub>();
            var cfg = DependencyInjection.Services.GetService<IUserConfiguration>();
            var sb = new SearchBoxViewModel(eh);
            var fd = new FolderDropDownViewModel();
            var flst = new FileListViewModel(eh);
            var lv = new LibraryViewModel(cfg, eh, sb, fd, flst); //DependencyInjection.Services.GetService<LibraryViewModel>();
            var vp = DependencyInjection.Services.GetService<VideoPlayerViewModel>();
            DataContext = new MainWindowViewModel(eh, lv, vp); // DependencyInjection.Services.GetService<MainWindowViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter is MainWindowViewModel vm)
            {
                DataContext = vm;
            }
        }
    }
}
