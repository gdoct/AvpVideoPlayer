using AvpVideoPlayer.Uwp.Api;
using AvpVideoPlayer.Uwp.Utility;
using AvpVideoPlayer.Uwp.ViewModels.Events;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace AvpVideoPlayer.Uwp.ViewModels;

public class PlayerControlsViewModel : EventBasedViewModel
{
    private bool _isMuted = false;
    private bool _isPlayButtonVisible = true;
    private bool _isPlaying = false;
    private bool _isSubtitlesVisible = false;
    private bool _isUpdatingFromEvent = false;
    private TimeSpan _playDuration;
    private double _position = 0;
    private double _previousvolume = 100;
    private double _volume = 100;
    private bool _isPanelVisible = true;
    private string _currentVideoFile;
    private bool _isRepeatVisible;
    private string _positionTooltip = "";
    private ImageSource _positionImage;

    public PlayerControlsViewModel(IEventHub eventHub, IViewRegistrationService viewRegistrationService) : base(eventHub)
    {
        _viewRegistrationService = viewRegistrationService;
//        _snapshotService = snapshotService;
        PlayCommand = new RelayCommand((_) => Play());
        PauseCommand = new RelayCommand((_) => Pause());
        StopCommand = new RelayCommand((_) => Stop());
        MuteCommand = new RelayCommand((_) => Mute());
        PreviousVideoCommand = new RelayCommand((_) => PreviousVideo());
        ToggleSubtitlesCommand = new RelayCommand((_) => ToggleSubtitles());
        ToggleRepeatCommand = new RelayCommand((_) => ToggleRepeat());
        NextVideoCommand = new RelayCommand((_) => NextVideo());
        MouseEnterCommand = new RelayCommand((_) => AnimatePanelUp());
        MouseLeaveCommand = new RelayCommand((_) => RunDelayed(AnimatePanelDown));
        PositionMouseOverCommand = new RelayCommand(OnPositionMouseOver);

        Subscribe<PlayStateChangedEvent>(e => UpdatePlayState(e.Data));
        Subscribe<ToggleSubtitlesEvent>(OnUpdateSubtitleState);
        Subscribe<ToggleRepeatEvent>(OnUpdateRepeatState);
        Subscribe<PlayPositionChangedEvent>(UpdatePlayPositionChanged);
        Subscribe<PlayDurationChangedEvent>(UpdatePlayDurationChanged);
        Subscribe<VolumeChangedEvent>(UpdateVolume);
        Subscribe<SubtitlesLoadedEvent>(OnSubtitlesLoaded);
        Subscribe<FullScreenEvent>(OnFullScreen);
        Subscribe<SelectVideoEvent>(OnSelectVideo);
    }


    private void OnSelectVideo(SelectVideoEvent e)
    {
        if (string.Compare(e.Data, _currentVideoFile, StringComparison.OrdinalIgnoreCase) != 0)
        {
            _currentVideoFile = e.Data;
            IsSubtitlesVisible = false;
            //_snapshotService.LoadVideofile(e.Data);
        }
    }

    public FrameworkElement AnimationTarget
    {
        get => (FrameworkElement)_viewRegistrationService.GetInstance(ViewResources.PlayerControls);
    }

    private void OnFullScreen(FullScreenEvent e)
    {
        if (e.IsFullScreen)
        {
            RunDelayed(AnimatePanelDown);
        }
        else
        {
            RunDelayed(AnimatePanelUp);
        }
    }

    private static void RunDelayed(Action action, int delay = 200)
    {
        Task.Run(async () =>
        {
            await Task.Delay(delay);
            action();
        });
    }

    public bool IsMuted
    {
        get => _isMuted;
        set { _isMuted = value; RaisePropertyChanged(); }
    }

    public bool IsPlaybuttonVisible
    {
        get => _isPlayButtonVisible;
        set { _isPlayButtonVisible = value; RaisePropertyChanged(); }
    }

    public bool IsPlaying
    {
        get => _isPlaying;
        set => SetProperty(ref _isPlaying, value);
    }

    public bool IsSubtitlesVisible
    {
        get => _isSubtitlesVisible;
        set => SetProperty(ref _isSubtitlesVisible, value);
    }

    public bool IsRepeatEnabled
    {
        get => _isRepeatVisible;
        set => SetProperty(ref _isRepeatVisible, value);
    }

    public string PositionTooltip { get => _positionTooltip; set => SetProperty(ref _positionTooltip, value); }
    public ImageSource PositionImage { get => _positionImage; set => SetProperty(ref _positionImage, value); }

    public ICommand MuteCommand { get; }
    public ICommand NextVideoCommand { get; }
    public ICommand PauseCommand { get; }
    public ICommand PositionMouseOverCommand { get; }

    private readonly IViewRegistrationService _viewRegistrationService;
    //private readonly ISnapshotService _snapshotService;

    public ICommand PlayCommand { get; }

    public TimeSpan PlayDuration
    {
        get => _playDuration;
        set
        {
            _playDuration = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(PlayDurationLabelText));
            RaisePropertyChanged(nameof(PlayDurationInMilliSeconds));
        }
    }

    private void AnimatePanelDown()
    {
        if (!_isPanelVisible) return;
        _isPanelVisible = false;
        AnimatePanel(1, 0, TimeSpan.FromSeconds(1));
    }
    private void AnimatePanelUp()
    {
        if (_isPanelVisible) return;
        _isPanelVisible = true;
        AnimatePanel(0, 1, TimeSpan.FromMilliseconds(100));
    }

    private void OnPositionMouseOver(object _)
    {
        //if (obj is not MouseEventArgs e) return;
        //var positionslider = (Slider) _viewRegistrationService.GetInstance(ViewResources.PositionSlider);
        //if (positionslider == null) return;
        //if (!_isPlaying)
        //{
        //    PositionTooltip = "";
        //    return;
        //}
        //var position = e.GetPosition(positionslider);
        //var perc = position.X / positionslider.RenderSize.Width;
        //var time = TimeSpan.FromMilliseconds(_playDuration.TotalMilliseconds * perc);
        //PositionTooltip = time.ToString(Resources.TimespanFormat);
        //PositionImage = _snapshotService.GetBitmapForTime(time);
    }

    private void AnimatePanel(int from, int to, TimeSpan duration) // 0-299
    {
        /*
         <DoubleAnimation 
                Storyboard.TargetName="MyAnimatedBrush"
                Storyboard.TargetProperty="(Brush.Opacity)" 
                From="1" To="0" Duration="0:0:5" AutoReverse="True"  />  
         */

        var animation = new DoubleAnimation() {  From = from, To = to, Duration = duration };
        if (animation.Duration.TimeSpan.TotalMilliseconds < 0)
        {
            System.Diagnostics.Debug.WriteLine("");
        }
      //  AnimationTarget.StartAnimation(FrameworkElement.OpacityProperty, animation);
    }

    public double Position
    {
        get => _position;
        set
        {
            if (WasPositionChangedByUser(value))
            {
                Publish(new PlayPositionChangeRequestEvent(value));
            }
            else
            {
                _isUpdatingFromEvent = true;
                _position = value;
                RaisePropertyChanged();
                _isUpdatingFromEvent = false;
            }
            RaisePropertyChanged(nameof(PositionLabelText));
        }
    }

    public ICommand PreviousVideoCommand { get; }
    public ICommand StopCommand { get; }
    public ICommand ToggleSubtitlesCommand { get; }
    public ICommand ToggleRepeatCommand { get; }

    public double Volume
    {
        get => _volume;
        set
        {
            if (WasVolumeChangedByUser(value))
            {
                Publish(new VolumeChangeRequestEvent(value));
            }
            else
            {
                _isUpdatingFromEvent = true;
                _volume = value;
                RaisePropertyChanged();
                _isUpdatingFromEvent = false;
            }
        }
    }

    public double PlayDurationInMilliSeconds => PlayDuration.TotalMilliseconds;

    public string PlayDurationLabelText =>
        _playDuration.ToString(GetTranslation("TimespanFormat"));

    public string PositionLabelText =>
        TimeSpan.FromMilliseconds(Position).ToString(GetTranslation("TimespanFormat"));

    private void Mute()
    {
        if (IsMuted)
        {
            Publish(new VolumeChangeRequestEvent(_previousvolume));
        }
        else
        {
            _previousvolume = Volume;
            Publish(new VolumeChangeRequestEvent((double)0));
        }
    }

    private void NextVideo()
    {
        Publish(new PlaylistMoveEvent(PlayListMoveTypes.Forward));
    }

    private void OnSubtitlesLoaded(SubtitlesLoadedEvent e)
    {

    }
    
    private void OnUpdateRepeatState(ToggleRepeatEvent e)
    {
        if (e?.IsHandled == true)
        {
            IsRepeatEnabled = e.Data;
        }
    }

    private void OnUpdateSubtitleState(ToggleSubtitlesEvent e)
    {
        if (e?.IsHandled == true)
        {
            IsSubtitlesVisible = e.Data;
        }
    }

    private void Pause()
    {
        Publish(new PlayStateChangeRequestEvent(PlayStates.Pause));
    }

    private void Play()
    {
        Publish(new PlayStateChangeRequestEvent(PlayStates.Play));
    }

    private void PreviousVideo()
    {
        Publish(new PlaylistMoveEvent(PlayListMoveTypes.Back));
    }

    private void Stop()
    {
        Publish(new PlayStateChangeRequestEvent(PlayStates.Stop));
    }

    private void ToggleSubtitles()
    {
        Publish(new ToggleSubtitlesEvent(!IsSubtitlesVisible));
    }

    private void ToggleRepeat()
    {
        Publish(new ToggleRepeatEvent(!IsRepeatEnabled));
    }

    private void UpdatePlayDurationChanged(PlayDurationChangedEvent e)
    {
        var max = e.Data;
        PlayDuration = TimeSpan.FromMilliseconds(max);
    }

    private void UpdatePlayPositionChanged(PlayPositionChangedEvent e)
    {
        _isUpdatingFromEvent = true;
        Position = e.Data;
        _isUpdatingFromEvent = false;
    }

    private void UpdatePlayState(PlayStates playstate)
    {
        IsPlaybuttonVisible = playstate is PlayStates.Pause or PlayStates.Stop;
        IsPlaying = !IsPlaybuttonVisible;
    }

    private void UpdateVolume(VolumeChangedEvent e)
    {
        _isUpdatingFromEvent = true;
        var newvolume = e.Data;
        if (newvolume > 0 && IsMuted)
        {
            IsMuted = false;
        }
        else if (newvolume == 0)
        {
            IsMuted = true;
        }
        Volume = newvolume;

        _isUpdatingFromEvent = false;
    }

    //private void VolumeSlider_ValueChanged(object sender,object e)
    //{
    //    //  RoutedPropertyChangedEventArgs<double>
    //    if (_isUpdatingFromEvent) return;
    //    Publish(new PlayPositionChangeRequestEvent((double) e));
    //}

    private bool WasPositionChangedByUser(double newvalue)
    {
        // no need to run code when it is changed by the timer
        if (_isUpdatingFromEvent) return false;
        if (Math.Abs(newvalue - Position) >= 1000)
        {
            return true;
        }
        return false;
    }

    private bool WasVolumeChangedByUser(double _)
    {
        if (_isUpdatingFromEvent) return false;
        return true;
    }

    public ICommand MouseEnterCommand { get; }

    public ICommand MouseLeaveCommand { get; }

}