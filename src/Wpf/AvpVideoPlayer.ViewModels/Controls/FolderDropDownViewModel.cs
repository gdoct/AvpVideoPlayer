using AvpVideoPlayer.Api;
using AvpVideoPlayer.Utility;
using AvpVideoPlayer.ViewModels.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AvpVideoPlayer.ViewModels.Controls;

public class FolderDropDownViewModel : INotifyPropertyChanged
{
    private string _currentPath = @"";
    private bool _loading = false;

    private FolderViewModel? _selectedFolderItem;
    private readonly IDispatcherHelper _dispatcherHelper;

    public FolderViewModel? SelectedFolderItem { get => _selectedFolderItem; set => SetProperty(ref _selectedFolderItem, value); }
    public event EventHandler<string>? SelectedFolderChanged;

    public FolderDropDownViewModel(IDispatcherHelper dispatcherHelper)
    {
        _dispatcherHelper = dispatcherHelper;
    }

    public ObservableCollection<FolderViewModel> Folders { get; } = new ObservableCollection<FolderViewModel>();
    public string CurrentPath
    {
        get => _currentPath;
        set
        {
            if (_loading) return;
            if (_currentPath == value) return;
            _loading = true;
            _dispatcherHelper.Invoke(() =>
            {
                SetProperty(ref _currentPath, value);
                LoadFolder();
                var newsel = Folders.FirstOrDefault(f => f.Path == value);
                if (newsel != null) SelectedFolderItem = newsel;
            });
            _loading = false;
            SelectedFolderChanged?.Invoke(this, value);
        }
    }

    private void LoadFolder()
    {
        Folders.Clear();
        if (_currentPath is null)
        {
            foreach (var drive in DriveInfo.GetDrives())
                Folders.Add(new FolderViewModel(drive.RootDirectory.FullName));
            return;
        }

        var folder = new DirectoryInfo(_currentPath);
        var parent = folder.Parent;

        foreach (var root in DriveInfo.GetDrives().Select(d => d.RootDirectory.FullName))
        {
            Folders.Add(new FolderViewModel(root));

            if (root.Equals(folder.FullName))
            {
                continue;
            }

            if (root.Equals(folder.Root.FullName))
            {
                parent = ScanDrives(folder, parent);
            }
        }
    }

    private DirectoryInfo? ScanDrives(DirectoryInfo folder, DirectoryInfo? parent)
    {
        var folderlist = new List<FolderViewModel?> { new FolderViewModel(folder.FullName) };
        while (parent?.Parent != null)
        {
            folderlist.Add(new FolderViewModel(parent.FullName));
            parent = parent.Parent;
        }
        folderlist.Reverse();
        foreach (var f in folderlist)
        {
            if (f != null)
                Folders.Add(f);
        }

        return parent;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
    {
        if (!Equals(field, newValue))
        {
            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        return false;
    }
}
