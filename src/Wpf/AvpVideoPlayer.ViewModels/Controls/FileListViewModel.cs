﻿using AvpVideoPlayer.Api;
using AvpVideoPlayer.MetaData;
using AvpVideoPlayer.ViewModels.Events;
using AvpVideoPlayer.ViewModels.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace AvpVideoPlayer.ViewModels.Controls;

public class FileListViewModel : EventBasedViewModel
{
    private readonly CollectionViewSource listviewCollection;
    private readonly IMetaDataService _metaDataService;
    private readonly IM3UService _m3uservice;
    private readonly FileSystemWatcher _fileSystemWatcher;
    private int _selectedIndex;
    private string? _path;
    private string? _filter;
    private FileListListViewItem? _selectedItem = null;
    private List<ChannelInfo> _channels = new();
    private string _playlist = string.Empty;
    private readonly IDispatcherHelper _dispatcherHelper;

    public FileListViewModel(IEventHub eventHub, IMetaDataService metaDataService, IM3UService m3uservice,
                            IDispatcherHelper dispatcherHelper) : base(eventHub)
    {
        listviewCollection = new CollectionViewSource
        {
            Source = FolderContents
        };
        listviewCollection.Filter += ListView_Filter;
        _metaDataService = metaDataService ?? throw new ArgumentNullException(nameof(metaDataService));
        _m3uservice = m3uservice ?? throw new ArgumentNullException(nameof(m3uservice));
        _fileSystemWatcher = new FileSystemWatcher() { IncludeSubdirectories = true };
        _dispatcherHelper = dispatcherHelper;
        _fileSystemWatcher.Changed += FileSystemWatcher_Changed;
        _fileSystemWatcher.Renamed += FileSystemWatcher_Changed;
        _fileSystemWatcher.Created += FileSystemWatcher_Changed;
        _fileSystemWatcher.Deleted += FileSystemWatcher_Changed;
        ActivateFileCommand = new RelayCommand(OnActivateFile);
        SelectFileCommand = new RelayCommand(OnSelectFile);
        Subscribe<PlaylistMoveEvent>(OnPlayListPositionChanged);
        Subscribe<SelectVideoEvent>(OnActivateVideo);
        Subscribe<SearchTextChangedEvent>(OnSearchTextChanged);
        Subscribe<MetaDataUpdatedEvent>(OnMetaDataUpdated);
    }

    private void OnMetaDataUpdated(MetaDataUpdatedEvent e)
    {
        foreach (var item in FolderContents)
        {
            if (item.MetaData != null && item.MetaData.FullName == e.MetaData.FullName)
            {
                item.MetaData = e.MetaData;
                break;
            }
        }
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
            if (path != null && string.Compare(path, newfile.Path, StringComparison.OrdinalIgnoreCase) == 0)
            {
                cur.IsActivated = true;
                cur.IsSelected = true;
                ActivatedFile = cur.File;
            }
            else
            {
                if (cur.IsActivated) cur.IsActivated = false;
                if (cur.IsSelected) cur.IsSelected = false;
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
        _dispatcherHelper.Invoke(() => Refresh(false));
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
        var name = thumb.File?.Name?.ToUpperInvariant() ?? string.Empty;
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
            if (string.Equals(value, _path, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            _path = value;
            if (!string.IsNullOrEmpty(_path) && File.Exists(_path) && FileExtensions.IsPlaylist(_path))
            {
                _fileSystemWatcher.EnableRaisingEvents = false;
            }
            else if (!string.IsNullOrEmpty(_path) && Directory.Exists(_path))
            {
                _fileSystemWatcher.Path = _path;
                _fileSystemWatcher.EnableRaisingEvents = true;
            }
            else
            {
                _fileSystemWatcher.EnableRaisingEvents = false;
            }
            _dispatcherHelper.Invoke(() => LoadContentsIntoListView(value, true));
        }
    }

    public FileViewModel? ActivatedFile { get; private set; }

    public void Refresh(bool force)
    {
        LoadContentsIntoListView(_path, force);
    }

    private void LoadContentsIntoListView(string? path, bool force)
    {
        if (string.IsNullOrWhiteSpace(path)) return;
        if (File.Exists(path) && FileExtensions.IsPlaylist(path))
        {
            LoadPlaylistIntoListview(path);
        }
        else if (Directory.Exists(path))
        {
            LoadFolderContentsIntoListView(path, force);
        }
        else if (!string.IsNullOrWhiteSpace(_playlist) && path.StartsWith(_playlist, StringComparison.OrdinalIgnoreCase))
        {
            LoadM3uCategoryIntoView(path);
        }
    }

    private void LoadM3uCategoryIntoView(string path)
    {
        var location = path[(_playlist.Length + 1)..];
        var parts = location.Split(@"\");
        var category = parts.FirstOrDefault();
        if (string.IsNullOrEmpty(category)) return;
        var channels = _channels.Where(s => s.Group.Equals(category));
        FolderContents.Clear();
        foreach (var channel in channels)
        {
            if (string.IsNullOrWhiteSpace(channel.Name)
                || channel.Uri == null
                || !channel.Uri.IsWellFormedOriginalString())
            {
                continue;
            }
            FolderContents.Add(new FileListListViewItem(
                new VideoStreamViewModel(channel.Name, channel.Uri), new FileMetaData()));
        }
    }

    private void LoadPlaylistIntoListview(string path)
    {
        // a new play list is opened - display the categories

        _channels = _m3uservice.ParsePlaylist(path);
        _playlist = path;
        FolderContents.Clear();
        var categories = _channels.Select(s => s.Group).Distinct().ToList();
        foreach (var category in categories)
        {
            if (string.IsNullOrWhiteSpace(category)) continue;
            FolderContents.Add(new FileListListViewItem(new VideoStreamCategoryViewModel(category, System.IO.Path.Combine(_playlist, category)), new FileMetaData()));
        }
    }

    private void LoadFolderContentsIntoListView(string path, bool force = true)
    {
        _playlist = string.Empty;
        _channels = new();
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
            if (FileExtensions.IsPlaylist(file))
            {
                newlist.Add(new PlayListViewModel(file.FullName));
            }
            else
            {
                newlist.Add(new VideoFileViewModel(file));
            }
        }

        if (!force
        && newlist.TrueForAll(f => FolderContents.Any(fc => fc.File.Path == f.Path))
        && FolderContents.All(f => newlist.Exists(fc => fc.Path == f.File.Path))
        )
        {
            return;
        }

        FolderContents.Clear();
        foreach (var file in newlist)
        {
            var isActive = ActivatedFile?.Path != null && string.Compare(ActivatedFile?.Path, file.Path, StringComparison.OrdinalIgnoreCase) == 0;
            var metadata = _metaDataService.GetMetadata(file.Path ?? "");
            FolderContents.Add(new FileListListViewItem(file, metadata) { IsActivated = isActive });
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

    private void OnPlayListPositionChanged(PlaylistMoveEvent e)
    {
        FileListListViewItem[] itemsInView = listviewCollection.View.Cast<FileListListViewItem>().ToArray();
        bool canPlay = false;
        bool isActivatedItemFound = false;
        if (e.Data == PlayListMoveTypes.Forward)
        {
            PlayNextItem(itemsInView, ref canPlay, ref isActivatedItemFound);
        }
        else // backwards is implied here
        {
            PlayPreviousItem(itemsInView, ref canPlay, ref isActivatedItemFound);
        }
        if (!canPlay)
        {
            Publish(new PlayStateChangeRequestEvent(PlayStates.Stop));
        }
    }

    private void PlayPreviousItem(FileListListViewItem[] itemsInView, ref bool canPlay, ref bool isActivatedItemFound)
    {
        for (int i = itemsInView.Length - 1; i >= 0; i--)
        {
            var item = itemsInView[i];
            if (item.IsActivated)
            {
                isActivatedItemFound = true;
            }
            else if (isActivatedItemFound && item?.File?.FileInfo != null)
            {
                Publish(new SelectVideoEvent(item.File));
                canPlay = true;
                break;
            }

        }
    }

    private void PlayNextItem(FileListListViewItem[] itemsInView, ref bool canPlay, ref bool isActivatedItemFound)
    {
        for (int i = 0; i < itemsInView.Length; i++)
        {
            var item = itemsInView[i];
            if (item.IsActivated)
            {
                isActivatedItemFound = true;
            }
            else if (isActivatedItemFound && item.File is VideoStreamViewModel vs)
            {
                Publish(new SelectVideoEvent(vs));
                canPlay = true;
                break;
            }
            else if (item?.File?.FileInfo != null)
            {
                Publish(new SelectVideoEvent(item.File));
                canPlay = true;
                break;
            }
        }
    }
}
