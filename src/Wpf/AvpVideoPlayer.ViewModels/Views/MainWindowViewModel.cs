using AvpVideoPlayer.Api;
using AvpVideoPlayer.Localization.Properties;
using AvpVideoPlayer.Utility;
using AvpVideoPlayer.ViewModels.Controls;
using AvpVideoPlayer.ViewModels.Events;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace AvpVideoPlayer.ViewModels.Views;

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
        KeyboardController = isActualApp ? new KeyboardController(_eventHub) : null;
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

    internal static int ConvertWindowToFontSize(Size size)
    {
        var q = ((size.Width / size.Height < 1.777777) ?
            size.Height : size.Width) / 40d;
        // find aspect ratio constraining side
        // assume 16:9 which is ok for subtitiles
        return
        (int)Math.Min(64, Math.Max(8, q));
    }

    private void OnFullScreen(FullScreenEvent e)
    {
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
