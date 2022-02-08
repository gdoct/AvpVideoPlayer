using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AvpVideoPlayer.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public BaseViewModel()
    {

    }

    protected void RaisePropertyChanged([CallerMemberName] string? propertyname = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
    }

    protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (!Equals(field, newValue))
        {
            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        return false;
    }
}
