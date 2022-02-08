using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.Video.Subtitles;

public interface ISubtitleContext
{
    SubtitleInfo SubtitleInfo { get; }
    /// <summary>
    /// Gets a SubtitleItem for the current timestamp, or null
    /// </summary>
    /// <param name="time">current timestamp</param>
    /// <returns>a SubtitleItem for the current timestamp, or null</returns>
    SubtitleData? GetSubtitleForTime(TimeSpan time);

    /// <summary>
    /// Gets the next subtile which would be displayed after the current timestamp
    /// when no subtitle is shown, the timestamp of the next subtitle needs to be 
    /// available to prevent polling the service.
    /// </summary> 
    /// <param name="time">current timestamp</param>
    /// <returns>a SubtitleItem which comes after the current timestamp, or null</returns>
    SubtitleData? GetNextSubtitleForTime(TimeSpan time);
}
