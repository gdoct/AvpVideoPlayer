using System;
using System.Windows.Input;

namespace AvpVideoPlayer.Uwp.Utility;

public class RelayCommand : ICommand
{
    private readonly Func<object, bool> _canexecuteMethod;
    private readonly Action<object> _executeMethod;

    public RelayCommand(Action<object> executeMethod, Func<object, bool> canexecuteMethod = null)
    {
        _executeMethod = executeMethod;
        _canexecuteMethod = canexecuteMethod;
    }


    public RelayCommand(Action executeMethod, Func<object, bool> canexecuteMethod = null)
    {
        _executeMethod = (_) => executeMethod();
        _canexecuteMethod = canexecuteMethod;
    }


    public bool CanExecute(object parameter)
    {
        if (_canexecuteMethod != null)
        {
            return _canexecuteMethod(parameter);
        }
        else
        {
            return true;
        }
    }

    public event EventHandler CanExecuteChanged
    {
        add
        {
         //   CommandManager.RequerySuggested += value;
        }
        remove
        {
           // CommandManager.RequerySuggested -= value;
        }
    }

    public void Execute(object parameter)
    {
        _executeMethod(parameter);
    }

}
