using AvpVideoPlayer.Api;
using AvpVideoPlayer.Utility;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AvpVideoPlayer.ViewModels;

public class DialogBoxViewModel : BaseViewModel
{
    private bool _suppressDialog;

    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }

    public DialogBoxViewModel(IViewRegistrationService viewRegistrationService, string dialogtext, string dialogtitle, bool allowSuppress = true, IDialogService.DialogTypes dialogtype = IDialogService.DialogTypes.Information)
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
    private static Icon GetIcon(IDialogService.DialogTypes dialogtype)
    {
        return dialogtype switch
        {
            IDialogService.DialogTypes.Information => SystemIcons.Information,
            IDialogService.DialogTypes.Error => SystemIcons.Error,
            IDialogService.DialogTypes.Application => SystemIcons.Application,
            IDialogService.DialogTypes.WinLogo => SystemIcons.WinLogo,
            IDialogService.DialogTypes.Asterix => SystemIcons.Asterisk,
            IDialogService.DialogTypes.Exclamation => SystemIcons.Exclamation,
            IDialogService.DialogTypes.Hand => SystemIcons.Hand,
            IDialogService.DialogTypes.Question => SystemIcons.Question,
            IDialogService.DialogTypes.Shield => SystemIcons.Shield,
            IDialogService.DialogTypes.Warning => SystemIcons.Warning,
            _ => SystemIcons.Information,
        };
    }

    private static ImageSource ToImageSource(Icon icon)
    {
        return Imaging.CreateBitmapSourceFromHIcon(
            icon.Handle,
            Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());
    }

    public IDialogService.DialogResult DialogResult { get; private set; } = IDialogService.DialogResult.Ok;

    private void Ok(object? commandParameter)
    {
        if (SuppressDialog)
        {
            DialogResult = IDialogService.DialogResult.Ok & IDialogService.DialogResult.DoNotShowAgain;
        }
        else
        {
            DialogResult = IDialogService.DialogResult.Ok;
        }
        ((Window)_viewRegistrationService.GetInstance(ViewResources.DialogBox))?.Close();
    }

    private void Cancel(object? commandParameter)
    {
        if (SuppressDialog)
        {
            DialogResult = IDialogService.DialogResult.Cancel & IDialogService.DialogResult.DoNotShowAgain;
        }
        else
        {
            DialogResult = IDialogService.DialogResult.Cancel;
        }
        ((Window)_viewRegistrationService.GetInstance(ViewResources.DialogBox))?.Close();
    }
}
