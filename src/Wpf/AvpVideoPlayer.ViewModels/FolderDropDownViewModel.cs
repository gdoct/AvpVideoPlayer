using AvpVideoPlayer.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AvpVideoPlayer.ViewModels;

public class FolderDropDownViewModel : INotifyPropertyChanged
{
    private string _currentPath = @"";
    private bool _loading = false;

    private FolderViewModel? _selectedFolderItem;
    public FolderViewModel? SelectedFolderItem { get => _selectedFolderItem; set => SetProperty(ref _selectedFolderItem, value); }
    public event EventHandler<string>? SelectedFolderChanged;

    public FolderDropDownViewModel()
    {
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
            DispatcherHelper.Invoke(() =>
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
                Folders.Add(new FolderViewModel( drive.RootDirectory.FullName ));
            return;
        }

        var folder = new DirectoryInfo(_currentPath);
        var parent = folder.Parent;

        foreach (var drive in DriveInfo.GetDrives())
        {
            Folders.Add(new FolderViewModel(drive.RootDirectory.FullName ));

            if (drive.RootDirectory.FullName.Equals(folder.FullName)) continue;
            if (drive.RootDirectory.FullName.Equals(folder.Root.FullName))
            {
                var folderlist = new List<FolderViewModel?> { new FolderViewModel(folder.FullName )};
                while (parent?.Parent != null)
                {
                    folderlist.Add(new FolderViewModel(parent.FullName ));
                    parent = parent.Parent;
                }
                folderlist.Reverse();
                foreach (var f in folderlist)
                {
                    if (f != null)
                        Folders.Add(f);
                }
            }
        }
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
