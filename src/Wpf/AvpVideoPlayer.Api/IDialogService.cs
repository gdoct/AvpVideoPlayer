namespace AvpVideoPlayer.Api
{
    public interface IDialogService
    {
        public enum DialogTypes
        {
            None,
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
            None = 0,
            Ok = 1,
            Cancel = 2,
            DoNotShowAgain = 4,
        }

        DialogResult Show(string dialogtext,
                          string dialogtitle,
                          bool allowSuppress = true,
                          DialogTypes dialogtype = DialogTypes.Information);
    }
}