using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AvpVideoPlayer.Uwp.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public BaseViewModel()
    {

    }

    protected void RaisePropertyChanged([CallerMemberName] string propertyname = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
    }

    protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
    {
        if (!Equals(field, newValue))
        {
            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        return false;
    }

    protected static string GetTranslation(string key)
    {
        if (Windows.UI.Core.CoreWindow.GetForCurrentThread() != null)
        {
            return Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView().GetString(key);
        }
        return key;
    }

}
