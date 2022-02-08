using AvpVideoPlayer.ViewModels;
using System.Windows.Controls;

namespace AvpVideoPlayer.Wpf.Views;

/// <summary>
/// Interaction logic for SearchBox.xaml
/// </summary>
public partial class SearchBox : UserControl
{
    public SearchBox()
    {
        InitializeComponent();
        //DataContext = ServiceLocator.GetService<SearchBoxViewModel>();
    }
}
