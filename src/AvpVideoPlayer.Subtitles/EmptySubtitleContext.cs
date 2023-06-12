using AvpVideoPlayer.Api;

namespace AvpVideoPlayer.Subtitles;

/// <inheritdoc cref="ISubtitleContext"/>
internal class EmptySubtitleContext : ISubtitleContext
{
    public SubtitleInfo SubtitleInfo { get; } = new SubtitleInfo();

    /// <inheritdoc cref="ISubtitleContext.GetNextSubtitleForTime(TimeSpan)"/>
    public SubtitleData? GetNextSubtitleForTime(TimeSpan time)
    {
        return null;
    }

    /// <inheritdoc cref="ISubtitleContext.GetSubtitleForTime(TimeSpan)"/>
    public SubtitleData? GetSubtitleForTime(TimeSpan time)
    {
        return null;
    }
}
