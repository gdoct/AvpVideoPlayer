using AvpVideoPlayer.ViewModels;
using System.Windows;
using System.Windows.Controls;


namespace AvpVideoPlayer.Wpf.Views;

/// <summary>
/// Interaction logic for DetailedVideoThumbnail.xaml
/// </summary>
public partial class DetailedFileThumbnail : UserControl
{
    public DetailedFileThumbnail()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty FileDependencyProperty
= DependencyProperty.Register("File",
                              typeof(FileViewModel),
                              typeof(DetailedFileThumbnail),
                              new PropertyMetadata(new FileViewModel(Api.FileTypes.Video),
                              new PropertyChangedCallback(OnFileChanged)));
    public FileViewModel File
    {
        set => SetValue(FileDependencyProperty, value);
        get => (FileViewModel)GetValue(FileDependencyProperty);
    }

    private static void OnFileChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
    }
}
