using AvpVideoPlayer.Api;
using AvpVideoPlayer.Utility;
using AvpVideoPlayer.ViewModels.Events;
using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;

namespace AvpVideoPlayer.ViewModels;

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

    public LibraryViewModel(IUserConfiguration userSettingsService, IEventHub eventHub, IDialogService dialogService, SearchBoxViewModel searchBoxViewModel, FolderDropDownViewModel folderDropDownViewModel, FileListViewModel fileListViewModel) : base(eventHub)
    {
        _userSettingsService = userSettingsService;
        _dialogService = dialogService;
        _searchBoxViewModel = searchBoxViewModel;
        _folderDropDownViewModel = folderDropDownViewModel;
        _folderDropDownViewModel.SelectedFolderChanged += FolderDropDownViewModel_PathChanged;
        _fileListViewModel = fileListViewModel;
        _fileListViewModel.SelectedFileChanged += OnSelectedFileChanged;
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
        if (_fullscreen) return;
        var filename = _filename;
        if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename)) return;
        if (string.Compare(FileListViewModel.ActivatedFile?.Path, filename, true) != 0) return;
        var result = _dialogService.Show($@"Are you sure you want to delete ""{filename}""?", "Confirm delete", true, IDialogService.DialogTypes.Warning);
        if (result == IDialogService.DialogResult.Ok)
        {
            if (_playstate != PlayStates.Stop)
            {
                Publish(new PlayStateChangeRequestEvent(PlayStates.Stop));
            }
            //nuke the file
            try
            {
                File.Delete(filename);
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
        FileListViewModel.Path = e;
    }


    private void OnSelectedFileChanged(object? sender, FileViewModel? e)
    {
        // this call is throttled and continued in OnSelectFile
        if (e != null)
        Publish(new SelectedFileChangedEvent(e));
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
            DispatcherHelper.Invoke(() =>
                LoadFolderContents(new DirectoryInfo(file.Path)));

        }
        else if (file is VideoFileViewModel)
        {
            if (file.FileType == FileTypes.Subtitles)
            {
                Publish(new LoadSubtitlesEvent(file.Path));
            }
            else
            {
                Publish(new SelectVideoEvent(file.Path));
            }
        }
    }

    private void OnSelectVideo(SelectVideoEvent e)
    {
        if (string.IsNullOrWhiteSpace(e.Data)) return;
        var fi = new FileInfo(e.Data);
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
