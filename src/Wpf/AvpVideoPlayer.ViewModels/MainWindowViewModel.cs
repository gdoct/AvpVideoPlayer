using AvpVideoPlayer.Api;
using AvpVideoPlayer.Localization.Properties;
using AvpVideoPlayer.Utility;
using AvpVideoPlayer.ViewModels.Events;
using Microsoft.Xaml.Behaviors.Core;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace AvpVideoPlayer.ViewModels;

public class MainWindowViewModel : EventBasedViewModel
{
    private int _prevSz = 0;
    private string _title;
    private bool _isFullscreen;
    private WindowState _windowState;
    private WindowStyle _windowStyle = DEFAULT_WINDOW_STYLE;
    private const WindowStyle DEFAULT_WINDOW_STYLE = WindowStyle.ThreeDBorderWindow;
    public ICommand OnKeyDownCommand { get; }
    public KeyboardController? KeyboardController { get; }
    public LibraryViewModel LibraryViewModel { get; }
    public VideoPlayerViewModel VideoPlayerViewModel { get; }

    public MainWindowViewModel(IEventHub eventHub, LibraryViewModel libraryViewModel, VideoPlayerViewModel videoPlayerViewModel) : base(eventHub)
    {
        var isActualApp = Application.Current?.Dispatcher != null;
        Subscribe<FullScreenEvent>(OnFullScreen);
        Subscribe<SelectVideoEvent>(OnSelectVideo);
        _title = Resources.ApplicationName;
        OnKeyDownCommand = new ActionCommand(OnKeyDown);
        KeyboardController = isActualApp ? new KeyboardController(this._eventHub) : null;
        LibraryViewModel = libraryViewModel;
        VideoPlayerViewModel = videoPlayerViewModel;
    }

    public string Title { get => _title; set => SetProperty(ref _title, value); }

    public bool IsFullscreen
    {
        get => _isFullscreen;
        set
        {
            _isFullscreen = value;
            RaisePropertyChanged();
            RaisePropertyChanged(nameof(IsTopMost));
        }
    }
    public bool IsTopMost => !System.Diagnostics.Debugger.IsAttached && _isFullscreen;

    public WindowState WindowState { get => _windowState; set => SetProperty(ref _windowState, value); }

    public WindowStyle WindowStyle { get => _windowStyle; set => SetProperty(ref _windowStyle, value); }

    private static int ConvertWindowToFontSize(Size size)
    {
        if (size.Width < 400 || size.Height < 400) return 12;
        if (size.Width < 1000 || size.Height < 1000) return 30;
        if (size.Width < 1500 || size.Height < 1500) return 48;
        return 64;
    }

    private void OnFullScreen(FullScreenEvent e)
    {
        this.Log($"MainWindowViewModel::OnFullScreen {nameof(e.IsFullScreen)}={e.IsFullScreen}, local={IsFullscreen}");
        if (e.IsFullScreen != IsFullscreen)
        {
            if (e.Data)
            {
                IsFullscreen = true;
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
            }
            else
            {
                IsFullscreen = false;
                WindowStyle = DEFAULT_WINDOW_STYLE;
                WindowState = WindowState.Normal;
            }
            Publish(new FullScreenEvent(IsFullscreen));
        }
    }

    public void OnKeyDown(object o)
    {
        if (o is not KeyEventArgs e) return;
        if (e.Handled) return;
        System.Diagnostics.Debug.Print("**** HANDLING KEYPRESS *****");
        e.Handled = KeyboardController?.ProcessKeypress(e.Key) ?? false;
    }

    public void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        var sz = ConvertWindowToFontSize(sizeInfo.NewSize);
        if (sz != _prevSz)
        {
            _prevSz = sz;
            Publish(new SetSubtitleSizeEvent(sz));
        }
    }

    private void OnSelectVideo(SelectVideoEvent e)
    {
        var name = e.Data.Name;
        Title = $"{Resources.ApplicationName} - {name}";
    }
}
