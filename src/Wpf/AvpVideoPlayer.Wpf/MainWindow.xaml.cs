using AvpVideoPlayer.ViewModels;
using System.Windows;

namespace AvpVideoPlayer.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        DataContext = mainWindowViewModel;
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        //var aspect = this.MediaPlayer.ViewBox.ActualWidth / this.MediaPlayer.ViewBox.ActualHeight;
        //if (sizeInfo.WidthChanged) this.Width = sizeInfo.NewSize.Height * aspect;
        //else this.Height = sizeInfo.NewSize.Width / aspect;

        ((MainWindowViewModel)DataContext).OnRenderSizeChanged(sizeInfo);
        if (WindowStyle == WindowStyle.None)
        {
            Library.Height = ActualHeight - 30;
        }
        else
        {
            Library.Height = ActualHeight - 50;
        }
    }
}
