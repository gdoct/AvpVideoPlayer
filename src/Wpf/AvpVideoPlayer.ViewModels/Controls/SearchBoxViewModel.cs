using AvpVideoPlayer.Api;
using AvpVideoPlayer.ViewModels.Events;
using AvpVideoPlayer.Utility;
using System.Windows.Input;
using System;

namespace AvpVideoPlayer.ViewModels.Controls;

public class SearchBoxViewModel : EventBasedViewModel
{
    private string _searchText = "";

    public ICommand? ClearCommand { get; }

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            RaisePropertyChanged();
            OnTextChanged();
            RaisePropertyChanged(nameof(HasText));
        }
    }

    public SearchBoxViewModel(IEventHub eventHub) : base(eventHub)
    {
        ClearCommand = new RelayCommand(Clear);
        Subscribe<PathChangedEvent>(OnPathChanged);
    }

    private void OnPathChanged(PathChangedEvent obj)
    {
        SearchText = "";
    }

    public bool HasText => !string.IsNullOrWhiteSpace(SearchText);

    private void Clear(object? obj)
    {
        SearchText = string.Empty;
    }

    private void OnTextChanged()
    {
        Publish(new SearchTextChangedEvent(SearchText));
    }
}
