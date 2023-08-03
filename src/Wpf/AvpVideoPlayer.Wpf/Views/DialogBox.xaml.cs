using AvpVideoPlayer.Wpf.Logic;
using System.Windows;

namespace AvpVideoPlayer.Wpf.Views;

/// <summary>
/// Interaction logic for DialogBox.xaml
/// </summary>
public partial class DialogBox : Window
{
    public DialogBox()
    {
        InitializeComponent();
        ViewRegistrationService.Instance.Register("DialogBox", this);
    }
}
