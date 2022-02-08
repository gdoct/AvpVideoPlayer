using AvpVideoPlayer.Api;
using AvpVideoPlayer.Utility;
using AvpVideoPlayer.ViewModels.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace AvpVideoPlayer.ViewModels;

public class FileListViewModel : EventBasedViewModel
{
    private readonly CollectionViewSource listviewCollection;
    private readonly FileSystemWatcher _fileSystemWatcher;
    private int _selectedIndex;
    private string? _path;
    private string? _filter;
    private FileListListViewItem? _selectedItem = null;

    public FileListViewModel(IEventHub eventHub) : base(eventHub)
    {
        listviewCollection = new CollectionViewSource
        {
            Source = FolderContents
        };
        listviewCollection.Filter += ListView_Filter;

        _fileSystemWatcher = new FileSystemWatcher() { IncludeSubdirectories = true };
        _fileSystemWatcher.Changed += FileSystemWatcher_Changed;
        _fileSystemWatcher.Renamed += FileSystemWatcher_Changed;
        _fileSystemWatcher.Created += FileSystemWatcher_Changed;
        _fileSystemWatcher.Deleted += FileSystemWatcher_Changed;
        ActivateFileCommand = new RelayCommand(OnActivateFile);
        SelectFileCommand = new RelayCommand(OnSelectFile);
        Subscribe<PlaylistMoveEvent>(OnPlayListChanged);
        Subscribe<SelectVideoEvent>(OnActivateVideo);
        Subscribe<SearchTextChangedEvent>(OnSearchTextChanged);
    }

    private void OnSearchTextChanged(SearchTextChangedEvent e)
    {
        Filter = e.Data ?? string.Empty;
        Refresh(true);
    }

    private void OnActivateVideo(SelectVideoEvent obj)
    {
        var newfile = obj.Data;
        foreach (var cur in FolderContents)
        {
            string? path = cur.File?.FileInfo?.FullName;
            if (path != null && string.Compare(path, newfile, StringComparison.OrdinalIgnoreCase) == 0)
            {
                cur.IsActivated = true;
                cur.IsSelected = true;
                ActivatedFile = cur.File;
            }
            else
            {
                if (cur.IsActivated) cur.IsActivated = false;
                if (cur.IsSelected) cur.IsSelected= false;
            }
        }
    }

    private void OnSelectFile(object? obj)
    {
        foreach (var cur in FolderContents.Where(f => f.IsSelected))
        {
            cur.IsSelected = false;
        }
        if (obj is FileListListViewItem fi)
        {
            fi.IsSelected = true;
        }
    }

    public ICommand ActivateFileCommand { get; }

    public ICommand SelectFileCommand { get; }

    private void OnActivateFile(object? commandParameter)
    {
        var vm = commandParameter as FileListListViewItem;
        SelectedFileChanged?.Invoke(this, vm?.File);
    }

    public ICollectionView SourceCollection => listviewCollection.View;

    public ObservableCollection<FileListListViewItem> FolderContents { get; } = new ObservableCollection<FileListListViewItem>();

    public event EventHandler<FileViewModel?>? SelectedFileChanged;

    public FileListListViewItem? SelectedItem
    {
        get
        {
            return _selectedItem;
        }
        set
        {
            _selectedItem = value;
            RaisePropertyChanged();
        }
    }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set { _selectedIndex = value; RaisePropertyChanged(); }
    }


    private void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
    {
        DispatcherHelper.Invoke(() => Refresh(false));
    }

    private void ListView_Filter(object sender, FilterEventArgs e)
    {
        if (string.IsNullOrEmpty(Filter))
        {
            e.Accepted = true;
            return;
        }
        if (e.Item is not FileListListViewItem thumb)
        {
            e.Accepted = false;
            return;
        }
        var name = thumb.File?.Name?.ToUpperInvariant() ?? String.Empty;
        if (name.Equals("..") || name.Contains(Filter.ToUpperInvariant()))
        {
            e.Accepted = true;
        }
        else
        {
            e.Accepted = false;
        }
    }

    public string? Filter
    {
        get
        {
            return _filter;
        }
        set
        {
            _filter = value;
            RaisePropertyChanged();
        }
    }

    public string? Path
    {
        get => _path;
        set
        {
            if (string.Compare(value, _path, StringComparison.OrdinalIgnoreCase) == 0) return;
            _path = value;
            if (!string.IsNullOrEmpty(_path) && Directory.Exists(_path))
            {
                _fileSystemWatcher.Path = _path;
                _fileSystemWatcher.EnableRaisingEvents = true;
            }
            else
            {
                _fileSystemWatcher.EnableRaisingEvents = false;
            }
            DispatcherHelper.Invoke(() => LoadFolderContentsIntoListView(value, true));
        }
    }

    public FileViewModel? ActivatedFile { get; private set; }

    public void Refresh(bool force)
    {
        LoadFolderContentsIntoListView(_path, force);
    }

    private void LoadFolderContentsIntoListView(string? path, bool force = true)
    {
        if (string.IsNullOrWhiteSpace(path)) return;
        var dir = new DirectoryInfo(path);
        if (!dir.Exists) return;

        IEnumerable<FileInfo> files;
        try
        {
            files = dir.GetFiles().Where(FileExtensions.IsValidFile);
        }
        catch (UnauthorizedAccessException)
        {
            return;
        }

        var newlist = new List<FileViewModel>();
        if (dir.Parent != null)
        {
            newlist.Add(new FolderViewModel(dir.Parent.FullName, ".."));
        }

        foreach (var folder in dir.GetDirectories())
        {
            newlist.Add(new FolderViewModel(folder.FullName));
        }
        foreach (var file in files)
        {
            newlist.Add(new VideoFileViewModel(file));
        }

        if (!force
            && newlist.All(f => FolderContents.Any(fc => fc.File.Path == f.Path))
            && FolderContents.All(f => newlist.Any(fc => fc.Path == f.File.Path))
            )
        {
            return;
        }

        FolderContents.Clear();
        foreach (var file in newlist)
        {
            var isActive = (ActivatedFile?.Path != null && string.Compare(ActivatedFile?.Path, file.Path, StringComparison.OrdinalIgnoreCase) == 0);
            FolderContents.Add(new FileListListViewItem(file) { IsActivated = isActive });
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _fileSystemWatcher?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void OnPlayListChanged(PlaylistMoveEvent e)
    {
        FileListListViewItem[] itemsInView = listviewCollection.View.Cast<FileListListViewItem>().ToArray();
        bool canPlay = false;
        bool isActivatedItemFound = false;
        if (e.Data == PlayListMoveTypes.Forward)
        {
            for (int i = 0; i < itemsInView.Length; i++)
            {
                var item = itemsInView[i];
                if (item.IsActivated)
                {
                    isActivatedItemFound = true;
                }
                else if (!isActivatedItemFound || item?.File?.FileInfo == null)
                {
                    continue;
                }
                else
                {
                    Publish(new SelectVideoEvent(item.File.FileInfo.FullName));
                    canPlay = true;
                    break;
                }

            }
        }
        else // backwards is implied here
        {
            for (int i = itemsInView.Length - 1; i >= 0 ; i--)
            {
                var item = itemsInView[i];
                if (item.IsActivated)
                {
                    isActivatedItemFound = true;
                }
                else if (!isActivatedItemFound || item?.File?.FileInfo == null)
                {
                    continue;
                }
                else
                {
                    Publish(new SelectVideoEvent(item.File.FileInfo.FullName));
                    canPlay = true;
                    break;
                }

            }
        }
        if (!canPlay)
        {
            Publish(new PlayStateChangeRequestEvent(PlayStates.Stop));
        }
    }

    public class FileListListViewItem : BaseViewModel
    {
        private bool _isSelected;
        private bool _isActivated;

        public FileListListViewItem(FileViewModel file)
        {
            File = file;
        }

        public FileViewModel File { get; }
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                RaisePropertyChanged();
            }
        }

        public bool IsActivated
        {
            get
            {
                return _isActivated;
            }
            set
            {
                _isActivated = value;
                RaisePropertyChanged();
            }
        }
    }
}
