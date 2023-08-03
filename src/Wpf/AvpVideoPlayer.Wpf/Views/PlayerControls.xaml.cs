using AvpVideoPlayer.Wpf.Logic;
using System.Windows.Controls;

namespace AvpVideoPlayer.Wpf.Views;

/// <summary>
/// Interaction logic for AutoHideControl.xaml
/// </summary>
public partial class PlayerControls : UserControl
{
    public PlayerControls()
    {
        InitializeComponent();
        // To get access to the grid containing the controls from within the MVVM viewmodel
        // (we need to run the fade-in/fade-out animations programmatically)
        // we register the instance through the IViewRegistrationService. The "Instance" singleton
        // is the same as the singleton registered with the DI framework. 
        ViewRegistrationService.Instance.Register(Api.ViewResources.PlayerControls, InnerGrid);
        ViewRegistrationService.Instance.Register(Api.ViewResources.PositionSlider, PositionSlider);
    }

    private void PositionSlider_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        PositionSliderTooltip.HorizontalOffset = e.GetPosition((System.Windows.IInputElement)sender).X + 10;
    }
}
