using Microsoft.Xaml.Behaviors.Core;
using System.Collections.ObjectModel;
using AvpVideoPlayer.MetaData;
using System.Windows.Input;
using AvpVideoPlayer.Utility;
using AvpVideoPlayer.Api;
using AvpVideoPlayer.ViewModels.Events;
using System.Windows;

namespace AvpVideoPlayer.ViewModels.Controls;

public class TagEditorViewModel : EventBasedViewModel
{
    private readonly Window _self;
    private readonly ITaggingService _taggingService;
    private string? _selectedTag;
    private string _newTag = "";

    public TagEditorViewModel(Window self, IEventHub eventhub, ITaggingService taggingService) : base(eventhub)
    {
        _self = self;
        _taggingService = taggingService;
        Tags = new ObservableCollection<string>(_taggingService.GetTags());

        ApplyCommand = new RelayCommand(_ => Apply());
        ResetCommand = new RelayCommand(_ => Cancel());
        AddCommand = new RelayCommand(_ => AddTag());
        RemoveCommand = new RelayCommand(_ => RemoveTag());
    }

    public ICommand RemoveCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand ApplyCommand { get; }
    public ICommand ResetCommand { get; }

    public ObservableCollection<string> Tags { get; }

    public string? SelectedTag { get => _selectedTag; set => SetProperty(ref _selectedTag, value); }

    public string NewTag { get => _newTag; set => SetProperty(ref _newTag, value); }


    public void Load()
    {
        Tags.Clear();
        var tags = _taggingService.GetTags();
        foreach (var tag in tags)
        {
            Tags.Add(tag);
        }
    }

    public void Apply()
    {
        var tags = _taggingService.GetTags();
        var isDirty = false;
        foreach (var tag in tags)
        {
            if (!Tags.Contains(tag))
            {
                isDirty = true;
                _taggingService.Remove(tag);
            }
        }

        foreach (var tag in Tags)
        {
            if (!tags.Contains(tag))
            {
                isDirty |= true;
                _taggingService.Add(tag);
            }
        }
        if (isDirty)
        {
            Publish(new TagsChangedEvent());
        }
        _self.DialogResult = true;
        _self.Close();
    }

    private void Cancel()
    {
        _self.DialogResult = false;
        _self.Close();
    }

    private void RemoveTag()
    {
        if (!string.IsNullOrWhiteSpace(SelectedTag))
        {
            Tags.Remove(SelectedTag);
        }
    }

    private void AddTag()
    {
        if (!string.IsNullOrWhiteSpace(NewTag) && !Tags.Contains(NewTag))
        {
            Tags.Add(NewTag);
            NewTag = "";
        }
    }
}
