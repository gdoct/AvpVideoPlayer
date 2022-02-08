namespace AvpVideoPlayer.Api
{
    public interface IDialogService
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

        DialogResult Show(string dialogtext,
                          string dialogtitle,
                          bool allowSuppress = true,
                          DialogTypes dialogtype = DialogTypes.Information);
    }
}