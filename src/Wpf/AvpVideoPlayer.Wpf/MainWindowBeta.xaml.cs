using AvpVideoPlayer.ViewModels.Views;
using System.Windows;

namespace AvpVideoPlayer.Wpf;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindowBeta : Window
{
    public MainWindowBeta(MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        DataContext = mainWindowViewModel;
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
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
