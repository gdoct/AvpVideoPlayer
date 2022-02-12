using AvpVideoPlayer.Api;
using AvpVideoPlayer.Utility;
using AvpVideoPlayer.Video.Subtitles;
using AvpVideoPlayer.ViewModels.Events;
using Microsoft.Win32;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace AvpVideoPlayer.ViewModels;

public class VideoPlayerViewModel : EventBasedViewModel
{
    private SubtitleData _currentSubtitle = new(0, 0);
    private string? _filename = "";
    private bool _isPlayingFullscreen = false;
    private bool _isVideoVisible = false;
    private Cursor _mouseCursor = Cursors.Arrow;
    private PlayStates _playState = PlayStates.Stop;
    private double _subtitleFontSize = 40;
    private readonly DispatcherTimer _timer;
    private double _volume = 100;
    private readonly TimeSpan DEFAULT_INTERVAL = TimeSpan.FromSeconds(1);
    private readonly TimeSpan MOUSE_TIMEOUT = TimeSpan.FromSeconds(3);
    private readonly IViewRegistrationService _viewRegistrationService;
    private readonly IUserConfiguration _userConfiguration;
    private bool _repeat = false;

    public ICommand OnMouseMoveCommand { get; }
    public ICommand OnMediaOpenedCommand { get; }
    public ICommand OnPreviewDragOver { get; }
    public ICommand OnDrop { get; }

    public IVideoPlayerView? View => (IVideoPlayerView)_viewRegistrationService.GetInstance(ViewResources.MediaElement);

    public VideoPlayerViewModel(IEventHub eventHub, PlayerControlsViewModel playerControlsViewModel, IViewRegistrationService viewRegistrationService, IUserConfiguration userConfiguration) : base(eventHub)
    {
        VideoPlayerDoubleClickCommand = new RelayCommand(OnDoubleClick);
        _playerControlsViewModel = playerControlsViewModel;
        _timer = new DispatcherTimer() { Interval = DEFAULT_INTERVAL, IsEnabled = true };
        _timer.Tick += TimerTick;
        _viewRegistrationService = viewRegistrationService;
        _userConfiguration = userConfiguration;
        _repeat = _userConfiguration.Repeat;
        if (_repeat) Publish(new ToggleRepeatEvent(_repeat) { IsHandled = true });

        OnMouseMoveCommand = new ActionCommand(OnMouseMove);
        OnMediaOpenedCommand = new ActionCommand(OnMediaOpened);
        OnPreviewDragOver = new ActionCommand(PerformOnPreviewDragOver);
        OnDrop = new ActionCommand(PerformOnDrop);
        Subscribe<SelectVideoEvent>(LoadVideo);
        Subscribe<LoadSubtitlesEvent>(OnLoadSubtitle);
        Subscribe<SetSubtitleSizeEvent>(e => SubtitleFontSize = e.Data);
        Subscribe<ToggleSubtitlesEvent>(ToggleSubtitles);
        Subscribe<ToggleRepeatEvent>(ToggleRepeat);
        Subscribe<ActivateSubtitleEvent>(ActivateSubtitle);
        Subscribe<PlayStateChangeRequestEvent>(OnUpdatePlayState);
        Subscribe<PlayPositionChangeRequestEvent>(e => OnSetPosition(TimeSpan.FromMilliseconds(e.Data)));
        Subscribe<VolumeChangeRequestEvent>(e => OnSetVolume(e.Data));
        Subscribe<FullScreenEvent>(e => OnFullscreen(e));
    }


    public ObservableCollection<MenuItem> AvailableSubs { get; } = new ObservableCollection<MenuItem>();

    public PlayerControlsViewModel PlayerControlsViewModel => _playerControlsViewModel;

    public bool IsVideoVisible
    {
        get => _isVideoVisible;
        private set
        {
            _isVideoVisible = value;
            RaisePropertyChanged();
        }
    }

    public Cursor MouseCursor { get => _mouseCursor; set => SetProperty(ref _mouseCursor, value); }

    private PlayStates PlayState
    {
        get => _playState;
        set
        {
            SetProperty(ref _playState, value);
            Publish(new PlayStateChangedEvent(PlayState) { Filename = _filename });
        }
    }

    public SubtitleData Subtitle => _currentSubtitle;

    public double SubtitleFontSize
    {
        get => _subtitleFontSize;
        set
        {
            if (value == _subtitleFontSize) return;
            _subtitleFontSize = value;
            RaisePropertyChanged();
        }
    }

    private SubtitleService SubtitleService { get; } = new SubtitleService();

    public string Url { get => _filename ?? string.Empty; set { _filename = value; RaisePropertyChanged(); } }

    public ICommand VideoPlayerDoubleClickCommand { get; }

    private readonly PlayerControlsViewModel _playerControlsViewModel;

    public double Volume { get => _volume; set { _volume = value; RaisePropertyChanged(); } }

    private void ActivateSubtitle(ActivateSubtitleEvent e)
    {
        MouseCursor = Cursors.Wait;
        var newsub = e?.Data;
        SubtitleService.ActivateSubtitle(newsub);
        Publish(new ToggleSubtitlesEvent(SubtitleService.IsSubtitleActive) { IsHandled = true, ActiveSubtitle = newsub });
        foreach (var item in AvailableSubs)
            if (item.IsChecked) item.IsChecked = false;
        var sub = (AvailableSubs.FirstOrDefault(s => ((SubtitleInfo?)s.Tag)?.StreamInfo == newsub?.StreamInfo));
        if (sub != null) sub.IsChecked = true;
        MouseCursor = Cursors.Arrow;
    }

    private void BrowseSubtitle()
    {
        var openFileDialog = new OpenFileDialog()
        {
            Filter = "Subtitles(*.srt; *.sub)|*.srt;*.sub|All files (*.*)|*.*",
            InitialDirectory = new FileInfo(Url).DirectoryName
        };
        if (openFileDialog.ShowDialog() == true)
        {
            Publish(new LoadSubtitlesEvent(openFileDialog.FileName));
        }
    }

    private void ClearSubtitle()
    {
        SubtitleService.ActivateSubtitle(null);
        foreach (var menuitem in AvailableSubs)
            if (menuitem.IsChecked) menuitem.IsChecked = false;
        var item = AvailableSubs.FirstOrDefault(s => string.Compare(s.Header.ToString(), "(None)", StringComparison.OrdinalIgnoreCase) == 0);
        if (item != null)
        {
            item.IsChecked = true;
        }
    }

    private void ClickSubtitle(SubtitleInfo info)
    {
        Publish(new ActivateSubtitleEvent(info));
    }

    private void LoadVideo(SelectVideoEvent e)
    {
        if (e?.Data != null)
        {
            if (string.Compare(Url, e.Data, StringComparison.OrdinalIgnoreCase) == 0)
            {
                //move to 00:00:00
                if (PlayState != PlayStates.Stop)
                {
                    OnStop();
                    OnSetPosition(TimeSpan.Zero);
                }
            }
            else if (File.Exists(e.Data))
            {
                OnStop();
                Url = e.Data;
                SubtitleService.ClearSubtitles();
                OnAvailableSubtitlesChanged();
                OnPlay();
            }
        }
    }

    private void OnAvailableSubtitlesChanged()
    {
        DispatcherHelper.Invoke(() => OnAvailableSubtitlesChangedImpl());
    }
    private void OnAvailableSubtitlesChangedImpl()
    {
        var subs = SubtitleService.AvailableSubtitles.ToList();
        AvailableSubs.Clear();
        var current = SubtitleService.CurrentSubtitleInfo;
        if (subs.Any())
        {
            AvailableSubs.Add(new MenuItem
            {
                Header = Localization.Properties.Resources.DeactivateSubtitles,
                Command = new RelayCommand((_) => ClearSubtitle()),
                Tag = null,
                IsChecked = (current == null),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center
            });
            int i = 0;
            foreach (var item in subs)
            {
                AvailableSubs.Add(new MenuItem
                {
                    Header = $"[{++i}] {item.SubtitleName}",
                    Command = new RelayCommand((_) => ClickSubtitle(item)),
                    Tag = item,
                    IsChecked = (current?.StreamInfo == item.StreamInfo),
                    HorizontalContentAlignment = HorizontalAlignment.Left,
                    VerticalContentAlignment = VerticalAlignment.Center
                });
            }
        }
        else
        {
            AvailableSubs.Add(new MenuItem
            {
                Header = Localization.Properties.Resources.NoSubtitlesLoaded,
                IsEnabled = false,
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center
            });
        }
        AvailableSubs.Add(new MenuItem
        {
            Header = Localization.Properties.Resources.LoadSubtitleFromFile,
            Command = new RelayCommand((_) => BrowseSubtitle()),
            HorizontalContentAlignment = HorizontalAlignment.Left,
            VerticalContentAlignment = VerticalAlignment.Center
        });


        Publish(new SubtitlesLoadedEvent(subs));
    }

    private void OnDoubleClick(object? data)
    {
        if (data is not MouseButtonEventArgs e) return;
        if (e.ClickCount == 2)
        {
            Publish(new FullScreenEvent(!_isPlayingFullscreen));
        }
    }

    private void OnFullscreen(FullScreenEvent e)
    {
        _isPlayingFullscreen = e.Data;
    }

    private void OnLoadSubtitle(LoadSubtitlesEvent e)
    {
        Task.Run(() => SubtitleService.AddSubtitlesFromFile(e.Filename))
            .ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    var info = task.Result;
                    if (info != null)
                        Publish(new ActivateSubtitleEvent(info));
                    OnAvailableSubtitlesChanged();
                }
            });
    }

    private void OnPause()
    {
        View?.Pause();
        PlayState = PlayStates.Pause;
    }


    private void OnPlay()
    {
        View?.Play();
        PlayState = PlayStates.Play;
    }

    private void OnSetPosition(TimeSpan timespan)
    {
        if (View != null)
            View.Position = timespan;
        Publish(new PlayPositionChangedEvent(timespan.TotalMilliseconds));
    }

    private void OnSetVolume(double value)
    {
        Volume = value / 100;
        Publish(new VolumeChangedEvent(value));
    }

    private void OnStop()
    {
        View?.Stop();
        PlayState = PlayStates.Stop;
    }

    private void OnMouseMove()
    {
        MouseCursor = Cursors.Arrow;
    }

    private void OnUpdatePlayState(PlayStateChangeRequestEvent e)
    {
        PlayStates newstate = e.Data;
        switch (newstate)
        {
            case PlayStates.Play:
                OnPlay();
                break;
            case PlayStates.Pause:
                OnPause();
                break;
            case PlayStates.Stop:
                OnStop();
                break;
            case PlayStates.Toggle:
                if (PlayState == PlayStates.Play)
                {
                    OnPause();
                }
                else
                {
                    OnPlay();
                }

                break;
            default:
                throw new NotImplementedException();
        }
    }

    private void TimerTick(object? sender, EventArgs e)
    {
        if (View is null) return;
        if (PlayState == PlayStates.Play)
        {
            var position = View.Position;

            if (position >= View.NaturalDuration)
            {
                if (_repeat)
                {
                    position = TimeSpan.Zero;
                    View.Position = position;
                }
                else
                {
                    OnStop();
                }
            }

            Publish(new PlayPositionChangedEvent(position.TotalMilliseconds));
            var subtitle = SubtitleService.GetSubtitle(position);
            if (subtitle.StartTime != _currentSubtitle.StartTime)
            {
                _currentSubtitle = subtitle;
                RaisePropertyChanged(nameof(Subtitle));
            }

            if (IdleTimeDetector.GetIdleTimeInfo().IdleTime > MOUSE_TIMEOUT)
            {
                MouseCursor = Cursors.None;
            }
        }
    }

    private void ToggleRepeat(ToggleRepeatEvent e)
    {
        if (!e.IsHandled)
        {
            _repeat = !_repeat;
            _userConfiguration.Repeat = _repeat;
            Publish(new ToggleRepeatEvent(_repeat) { IsHandled = true });
        }
    }

    private void ToggleSubtitles(ToggleSubtitlesEvent e)
    {
        if (!e.IsHandled)
        {
            var isVisible = false;
            if (e.Data)
            {
                var sub = SubtitleService.AvailableSubtitles;
                if (sub.Any())
                {
                    SubtitleService.ActivateSubtitle(sub.First());
                    isVisible = true;
                }
            }
            else
            {
                SubtitleService.ActivateSubtitle(null);
            }
            Publish(new ToggleSubtitlesEvent(isVisible) { IsHandled = true });
        }
    }


    private void OnMediaOpened(object data)
    {
        if (View?.NaturalDuration == null) return;
        var duration = View.NaturalDuration;
        if (duration.TotalMilliseconds > 0)
        {
            IsVideoVisible = true;
            Publish(new PlayDurationChangedEvent(View.NaturalDuration.TotalMilliseconds));
            PlayState = PlayStates.Play;
            Task.Run(() => SubtitleService.AddSubtitlesFromFile(_filename))
            .ContinueWith(t =>
            {
                if (t.IsCompletedSuccessfully) OnAvailableSubtitlesChanged();
            });
        }
        else
        {
            IsVideoVisible = false;
        }
    }


    private void PerformOnPreviewDragOver(object e)
    {
        if (e is not DragEventArgs args) return;
        var argdata = args.Data;
        if (argdata == null || !argdata.GetDataPresent("FileDrop")) return;
        var droppedfiles = (string[])argdata.GetData("FileDrop");
        if (droppedfiles.Any(FileExtensions.IsValidFile))
        {
            args.Effects = DragDropEffects.All;
        }
        else
        {
            args.Effects = DragDropEffects.None;
            args.Handled = true;
        }
    }

    private void PerformOnDrop(object e)
    {
        if (e is DragEventArgs args)
        {
            var argdata = args.Data;
            if (argdata == null || !argdata.GetDataPresent("FileDrop")) return;
            var droppedfiles = (string[])argdata.GetData("FileDrop");
            foreach (var filename in droppedfiles)
            {
                var fi = new FileInfo(filename);
                if (fi.Exists)
                {
                    if (FileExtensions.IsVideoFile(fi))
                    {
                        Task.Run(() => Publish(new SelectVideoEvent(filename)));
                        break;
                    }
                    else if (FileExtensions.IsSubtitleFile(fi))
                    {
                        Task.Run(() => Publish(new LoadSubtitlesEvent(filename)));
                        break;
                    }
                }
            }
        }
    }
}
