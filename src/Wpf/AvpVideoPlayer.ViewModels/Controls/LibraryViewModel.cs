﻿using AvpVideoPlayer.Api;
using AvpVideoPlayer.ViewModels.Events;
using AvpVideoPlayer.ViewModels.IO;
using System;
using System.IO;
using System.Reactive.Linq;
using System.Windows;

namespace AvpVideoPlayer.ViewModels.Controls;

public class LibraryViewModel : EventBasedViewModel
{
    private int _selectedIndex;
    private readonly IUserConfiguration _userSettingsService;
    private readonly IDialogService _dialogService;
    private readonly SearchBoxViewModel _searchBoxViewModel;
    private string? _filename;
    private PlayStates _playstate;
    private bool _fullscreen = false;
    private readonly FolderDropDownViewModel _folderDropDownViewModel;
    private readonly FileListViewModel _fileListViewModel;
    private readonly IDispatcherHelper _dispatcherHelper;

    public LibraryViewModel(IUserConfiguration userSettingsService,
                            IEventHub eventHub,
                            IDialogService dialogService,
                            IDispatcherHelper dispatcherHelper,
                            SearchBoxViewModel searchBoxViewModel,
                            FolderDropDownViewModel folderDropDownViewModel,
                            FileListViewModel fileListViewModel) : base(eventHub)
    {
        _userSettingsService = userSettingsService;
        _dialogService = dialogService;
        _searchBoxViewModel = searchBoxViewModel;
        _folderDropDownViewModel = folderDropDownViewModel;
        _folderDropDownViewModel.SelectedFolderChanged += FolderDropDownViewModel_PathChanged;
        _fileListViewModel = fileListViewModel;
        _fileListViewModel.SelectedFileChanged += OnSelectedFileChanged;
        _dispatcherHelper = dispatcherHelper;
        RestoreUserPreferences();
        Subscribe<DeleteCurrentVideoEvent>(OnDeleteCurrentVideo);
        Subscribe<PlayStateChangedEvent>(UpdatePlayState);
        Subscribe<FullScreenEvent>(OnFullScreen);
        AddSubscription(EventsOfType<SelectVideoEvent>()
                                 .Throttle(TimeSpan.FromMilliseconds(100))
                                 .Subscribe(OnSelectVideo)
                       );

        AddSubscription(EventsOfType<SelectedFileChangedEvent>()
                         .Throttle(TimeSpan.FromMilliseconds(100))
                         .Subscribe(OnSelectFile)
               );
    }

    private void OnFullScreen(FullScreenEvent obj)
    {
        _fullscreen = obj.IsFullScreen;
    }

    private void UpdatePlayState(PlayStateChangedEvent e)
    {
        _filename = e.Filename;
        _playstate = e.Data;
    }

    public SearchBoxViewModel SearchBoxViewModel { get => _searchBoxViewModel; }
    public FolderDropDownViewModel FolderDropDownViewModel { get => _folderDropDownViewModel; }
    public FileListViewModel FileListViewModel { get => _fileListViewModel; }

    public int SelectedIndex
    {
        get => _selectedIndex;
        set { _selectedIndex = value; RaisePropertyChanged(); }
    }

    private void OnDeleteCurrentVideo(DeleteCurrentVideoEvent obj)
    {
        if (_fullscreen)
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(_filename) || !File.Exists(_filename) || Uri.IsWellFormedUriString(_filename, UriKind.Absolute) || string.Compare(FileListViewModel.ActivatedFile?.Path, _filename, true) != 0)
        {
            return;
        }

        var result = _dialogService.Show($@"Are you sure you want to delete ""{_filename}""?", "Confirm delete", true, IDialogService.DialogTypes.Warning);
        if (result == IDialogService.DialogResults.Ok)
        {
            if (_playstate != PlayStates.Stop)
            {
                Publish(new PlayStateChangeRequestEvent(PlayStates.Stop));
            }
            //nuke the file
            try
            {
                File.Delete(_filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to delete the file: {ex.Message}", "Error deleting file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private void LoadFolderContents(DirectoryInfo path)
    {
        FolderDropDownViewModel.CurrentPath = path.FullName;
    }

    private void FolderDropDownViewModel_PathChanged(object? _, string? e)
    {
        // this will update the list of files in view
        FileListViewModel.Path = e;
    }


    private void OnSelectedFileChanged(object? sender, FileViewModel? e)
    {
        // this call is throttled and continued in OnSelectFile
        if (e != null)
        {
            Publish(new SelectedFileChangedEvent(e));
        }
    }

    public void OnSelectFile(SelectedFileChangedEvent e)
    {
        var file = e.Data;
        if (file?.Path == null)
        {
            return;
        }
        if (file is FolderViewModel)
        {
            Publish(new PathChangedEvent(file.Path));
            _userSettingsService.LastPath = file.Path;
            _dispatcherHelper.Invoke(() =>
                LoadFolderContents(new DirectoryInfo(file.Path)));

        }
        else if (file is PlayListViewModel || file is VideoStreamCategoryViewModel)
        {
            _dispatcherHelper.Invoke(() =>
                LoadPlaylistContents(file));
        }
        else if (file is VideoFileViewModel)
        {
            if (file.FileType == FileTypes.Subtitles)
            {
                Publish(new LoadSubtitlesEvent(file.Path));
            }
            else
            {
                Publish(new SelectVideoEvent(file));
            }
        }
        else if (file is VideoStreamViewModel)
        {
            Publish(new SelectVideoEvent(file));
        }
    }

    private void LoadPlaylistContents(FileViewModel file)
    {
        FileListViewModel.Path = file.Path;
        FolderDropDownViewModel.CurrentPath = file.Path ?? string.Empty;
    }

    private void OnSelectVideo(SelectVideoEvent e)
    {
        if (string.IsNullOrWhiteSpace(e.Data.Path)) return;
        var fi = new FileInfo(e.Data.Path);
        if (!fi.Exists || fi.Directory == null) return;
        if (string.Compare(fi.Directory.FullName, FileListViewModel.Path, true) != 0)
        {
            LoadFolderContents(fi.Directory);
        }
    }

    private void RestoreUserPreferences()
    {
        var path = _userSettingsService.LastPath;
        if (string.IsNullOrWhiteSpace(path)) path = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        LoadFolderContents(new DirectoryInfo(path));
        SelectedIndex = -1;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _fileListViewModel?.Dispose();
            _searchBoxViewModel?.Dispose();
        }
        base.Dispose(disposing);
    }
}
