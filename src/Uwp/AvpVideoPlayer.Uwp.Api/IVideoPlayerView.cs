using System;

namespace AvpVideoPlayer.Uwp.Api
{

    /// <summary>
    /// Wrapper for a MediaElement control
    /// </summary>
    public interface IVideoPlayerView
    {
        TimeSpan NaturalDuration { get; }
        TimeSpan Position { get; set; }

        void Play();
        void Pause();
        void Stop();
    }
}