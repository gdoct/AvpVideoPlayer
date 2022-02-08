using AvpVideoPlayer.Api;
using AvpVideoPlayer.ViewModels;
using AvpVideoPlayer.Wpf.Views;
using static AvpVideoPlayer.Api.IDialogService;

namespace AvpVideoPlayer.Wpf.Services;

public class DialogService : IDialogService
{
    private readonly IViewRegistrationService _viewRegistrationService;

    public DialogService(IViewRegistrationService viewRegistrationService)
    {
        _viewRegistrationService = viewRegistrationService;
    }

    public DialogResult Show(string dialogtext,
                             string dialogtitle,
                             bool allowSuppress = true,
                             DialogTypes dialogtype = DialogTypes.Information)
    {
        var dlog = new DialogBoxViewModel(_viewRegistrationService, dialogtext, dialogtitle, allowSuppress, dialogtype);
        var win = new DialogBox { DataContext = dlog };
        win.ShowDialog();
        return dlog.DialogResult;
    }
}
