using AvpVideoPlayer.Uwp.Api;
using AvpVideoPlayer.Uwp.Utility;
using System;
using System.Drawing;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace AvpVideoPlayer.Uwp.ViewModels;

public class DialogBoxViewModel : BaseViewModel
{
    private bool _suppressDialog;

    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }

    public DialogBoxViewModel(IViewRegistrationService viewRegistrationService, string dialogtext, string dialogtitle, bool allowSuppress = true, DialogTypes dialogtype = DialogTypes.Information)
    {
        DialogText = dialogtext ?? throw new ArgumentNullException(nameof(dialogtext));
        DialogTitle = dialogtitle ?? throw new ArgumentNullException(nameof(dialogtitle));
        Icon = ToImageSource(GetIcon(dialogtype));
        OkCommand = new RelayCommand(Ok);
        CancelCommand = new RelayCommand(Cancel);
        CanSuppressDialog = allowSuppress ? Visibility.Visible : Visibility.Collapsed;
        _viewRegistrationService = viewRegistrationService;
    }
    public string DialogTitle { get; }
    public string DialogText { get; }
    public bool SuppressDialog { get => _suppressDialog; set => SetProperty(ref _suppressDialog, value); }
    public Visibility CanSuppressDialog { get; }

    private readonly IViewRegistrationService _viewRegistrationService;

    public ImageSource Icon { get; } = ToImageSource(SystemIcons.Application);
    private static Icon GetIcon(DialogTypes dialogtype)
    {
        return dialogtype switch
        {
            DialogTypes.Information => SystemIcons.Information,
            DialogTypes.Error => SystemIcons.Error,
            DialogTypes.Application => SystemIcons.Application,
            DialogTypes.WinLogo => SystemIcons.WinLogo,
            DialogTypes.Asterix => SystemIcons.Asterisk,
            DialogTypes.Exclamation => SystemIcons.Exclamation,
            DialogTypes.Hand => SystemIcons.Hand,
            DialogTypes.Question => SystemIcons.Question,
            DialogTypes.Shield => SystemIcons.Shield,
            DialogTypes.Warning => SystemIcons.Warning,
            _ => SystemIcons.Information,
        };
    }

    private static ImageSource ToImageSource(Icon _)
    {
        return null;
        //return Imaging.CreateBitmapSourceFromHIcon(
        //    icon.Handle,
        //    Int32Rect.Empty,
        //    BitmapSizeOptions.FromEmptyOptions());
    }

    public DialogResult DialogResult { get; private set; } = DialogResult.Ok;

    private void Ok(object commandParameter)
    {
        if (SuppressDialog)
        {
            DialogResult = DialogResult.Ok & DialogResult.DoNotShowAgain;
        }
        else
        {
            DialogResult = DialogResult.Ok;
        }
        ((Window)_viewRegistrationService.GetInstance(ViewResources.DialogBox))?.Close();
    }

    private void Cancel(object commandParameter)
    {
        if (SuppressDialog)
        {
            DialogResult = DialogResult.Cancel & DialogResult.DoNotShowAgain;
        }
        else
        {
            DialogResult = DialogResult.Cancel;
        }
        ((Window)_viewRegistrationService.GetInstance(ViewResources.DialogBox))?.Close();
    }
}
