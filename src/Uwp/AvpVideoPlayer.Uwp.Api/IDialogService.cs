using System;

namespace AvpVideoPlayer.Uwp.Api
{
    public enum DialogTypes
    {
        Information,
        Error,
        Application,
        WinLogo,
        Asterix,
        Exclamation,
        Hand,
        Question,
        Shield,
        Warning
    }

    [Flags]
    public enum DialogResult
    {
        Ok,
        Cancel,
        DoNotShowAgain,
    }

    public interface IDialogService
    {
        DialogResult Show(string dialogtext,
                          string dialogtitle,
                          bool allowSuppress = true,
                          DialogTypes dialogtype = DialogTypes.Information);
    }
}
