using AvpVideoPlayer.Uwp.Api;
using System;
using Windows.UI.Xaml.Controls;

namespace AvpVideoPlayer.Uwp.ViewModels;
public class VideoPlayerView : IVideoPlayerView
{
    public MediaElement MediaPlayer { get; }

    public VideoPlayerView(MediaElement mediaPlayer)
    {
        MediaPlayer = mediaPlayer;
    }

    public TimeSpan NaturalDuration => MediaPlayer.NaturalDuration.HasTimeSpan ?
        MediaPlayer.NaturalDuration.TimeSpan : TimeSpan.Zero;

    public TimeSpan Position
    {
        get => MediaPlayer.Position;
        set => MediaPlayer.Position = value;
    }

    public void Pause()
    {
        MediaPlayer.Pause();
    }

    public void Play()
    {
        MediaPlayer.Play();
    }

    public void Stop()
    {
        MediaPlayer.Stop();
    }
}